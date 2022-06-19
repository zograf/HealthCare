using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HealthCare.Domain.Services;

public class EquipmentRequestService : IEquipmentRequestService
{
    private readonly IEquipmentRequestRepository _equipmentRequestRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IRoomRepository _roomRepository;

    public EquipmentRequestService(IEquipmentRequestRepository equipmentRequestRepository,
        IEquipmentRepository equipmentRepository,
        IInventoryRepository inventoryRepository,
        IRoomRepository roomRepository)
    {
        _equipmentRequestRepository = equipmentRequestRepository;
        _equipmentRepository = equipmentRepository;
        _inventoryRepository = inventoryRepository;
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<EquipmentRequestDomainModel>> GetAll()
    {
        IEnumerable<EquipmentRequest> equipmentRequests = await _equipmentRequestRepository.GetAll();
        if (equipmentRequests == null)
            throw new DataIsNullException();

        List<EquipmentRequestDomainModel> results = new List<EquipmentRequestDomainModel>();
        foreach (EquipmentRequest item in equipmentRequests)
            results.Add(ParseToModel(item));

        return results;
    }

    public async Task<Dictionary<decimal, EquipmentDomainModel>> MakeMissingEquipment()
    {
        Dictionary<decimal, EquipmentDomainModel> result = new Dictionary<decimal, EquipmentDomainModel>();
        List<Equipment> equipment = (List<Equipment>) await _equipmentRepository.GetAll();
        foreach (Equipment item in equipment)
        {
            EquipmentDomainModel equipmentModel = EquipmentService.ParseToModel(item);
            if (!equipmentModel.IsDynamic) continue;
            if (!result.ContainsKey(equipmentModel.Id)) result.Add(equipmentModel.Id, equipmentModel);
        }
        return result;
    }

    public async Task<List<EquipmentDomainModel>> FilterMissingEquipment(Dictionary<decimal, EquipmentDomainModel> missingEquipment)
    {
        List<EquipmentDomainModel> result = new List<EquipmentDomainModel>();
        List<Inventory> inventories = (List<Inventory>)await _inventoryRepository.GetAll();
        foreach (Inventory item in inventories)
        {
            InventoryDomainModel inventoryModel = InventoryService.ParseToModel(item);
            if (!inventoryModel.Equipment.IsDynamic) continue;
            if (missingEquipment.ContainsKey(inventoryModel.EquipmentId) && item.Amount != 0) missingEquipment.Remove(inventoryModel.EquipmentId);
        }
        foreach (KeyValuePair<decimal, EquipmentDomainModel> pair in missingEquipment)
            result.Add(pair.Value);
        return result;
    }

    public async Task<IEnumerable<EquipmentDomainModel>> GetMissingEquipment()
    {
        Dictionary<decimal, EquipmentDomainModel> missingEquipment = await MakeMissingEquipment();
        return await FilterMissingEquipment(missingEquipment);
    }

    public async Task<IEnumerable<EquipmentRequestDomainModel>> OrderEquipment(IEnumerable<EquipmentRequestDTO> dtos)
    {
        List<EquipmentRequestDomainModel> result = new List<EquipmentRequestDomainModel>();
        foreach (EquipmentRequestDTO dto in dtos)
            result.Add(MakeEquipmentRequest(dto));
        _equipmentRequestRepository.Save();
        return result;
    }

    public EquipmentRequestDomainModel MakeEquipmentRequest(EquipmentRequestDTO dto)
    {
        EquipmentRequestDomainModel equipmentRequestModel = GetModelFromDto(dto);
        _ = _equipmentRequestRepository.Post(ParseFromModel(equipmentRequestModel));
        return equipmentRequestModel;
    }

    public EquipmentRequestDomainModel GetModelFromDto(EquipmentRequestDTO dto)
    {
        EquipmentRequestDomainModel equipmentRequestModel = new EquipmentRequestDomainModel
        {
            ExecutionTime = UtilityService.RemoveSeconds(DateTime.Now.AddDays(1)),
            IsExecuted = false,
            Amount = dto.Amount,
            EquipmentId = dto.EquipmentId
        };
        
        return equipmentRequestModel;
    }

    public async Task<IEnumerable<EquipmentRequestDomainModel>> DoAllOrders()
    {
        Room storage = await _roomRepository.GetRoomByName("storage");
        List<EquipmentRequest> equipmentRequest = 
            (List<EquipmentRequest>) await _equipmentRequestRepository.GetAll();
        List<EquipmentRequestDomainModel> result = new List<EquipmentRequestDomainModel>();
        DateTime now = DateTime.Now;
        foreach (var item in equipmentRequest)
        {
            if (UtilityService.MinDate(item.ExecutionTime, now) == now || item.IsExecuted) continue;
            ParseRequest(item, storage);
            result.Add(ParseToModel(item));
            item.IsExecuted = true;
            _equipmentRequestRepository.Update(item);
        }
        _ = _roomRepository.Update(storage);
        _roomRepository.Save();
        _equipmentRepository.Save();
        return result;
    }


    public void ParseRequest(EquipmentRequest equipmentRequest, Room storage)
    {
        foreach (Inventory item in storage.Inventories)
        {
            if (item.EquipmentId == equipmentRequest.EquipmentId)
            {
                item.Amount += equipmentRequest.Amount;
                return;
            }
        }

        InventoryDomainModel inventoryModel = GetInventoryModel(equipmentRequest, storage.Id);
        storage.Inventories.Add(InventoryService.ParseFromModel(inventoryModel));
    }
    
    public async Task<IEnumerable<RoomAndEquipmentDTO>> ShowRoomAndEquipment()
    {
        List<RoomAndEquipmentDTO> result = new List<RoomAndEquipmentDTO>();
        List<Room> rooms = (List<Room>) await _roomRepository.GetAll();
        List<EquipmentDomainModel> allEquipment = await GetAllDynamicEquipment();
        foreach (Room item in rooms)
        {
            RoomAndEquipmentDTO dto = new RoomAndEquipmentDTO { RoomName = item.RoomName };
            FillDto(dto, item, allEquipment);
            dto.Equipment.Sort((x, y) => x.Amount.CompareTo(y.Amount));
            result.Add(dto);
        }
        return result;
    }


    public void FillDto(RoomAndEquipmentDTO dto, Room room, List<EquipmentDomainModel> allEquipment)
    {
        dto.Equipment = new List<EquipmentForRoomDTO>();
        foreach (EquipmentDomainModel equipmentModel in allEquipment)
        {
            Boolean found = false;
            foreach (Inventory item in room.Inventories)
            {
                if (item.EquipmentId == equipmentModel.Id)
                {
                    found = true;
                    dto.Equipment.Add(new EquipmentForRoomDTO { EquipmentName = item.Equipment.Name, Amount = item.Amount });
                    break;
                }
            }
            if (!found)
                dto.Equipment.Add(new EquipmentForRoomDTO { EquipmentName = equipmentModel.Name, Amount = 0});
        }
    }

    public async Task<List<EquipmentDomainModel>> GetAllDynamicEquipment()
    {
        List<EquipmentDomainModel> result = new List<EquipmentDomainModel>();
        List<Equipment> equipments = (List<Equipment>) await _equipmentRepository.GetAll();
        foreach (Equipment item in equipments)
        {
            if (!item.IsDynamic) continue;
            result.Add(EquipmentService.ParseToModel(item));
        }

        return result;
    }

    public InventoryDomainModel GetInventoryModel(EquipmentRequest equipmentRequest, decimal roomId)
    {
        InventoryDomainModel inventoryModel = new InventoryDomainModel
        {
            Amount = equipmentRequest.Amount,
            EquipmentId = equipmentRequest.EquipmentId,
            IsDeleted = false,
            RoomId = roomId
        };
        
        return inventoryModel;
    }
    
    public async Task<EquipmentDomainModel> TransferEquipment(TransferEquipmentDTO dto)
    {
        EquipmentDomainModel equipmentModel;
        Room roomFrom = await _roomRepository.GetRoomById(dto.FromRoomId);
        Room roomTo = await _roomRepository.GetRoomById(dto.ToRoomId);
        try
        {
            equipmentModel = await TryTransfer(roomFrom, roomTo, dto);
            _roomRepository.Update(roomTo);
            _roomRepository.Update(roomFrom);
            _roomRepository.Save();
        }
        catch (Exception exception)
        {
            throw new NotEnoughResourcesForTransfer();
        }

        return equipmentModel;
    }

    public async Task<EquipmentDomainModel?> TryTransfer(Room roomFrom, Room roomTo, TransferEquipmentDTO dto)
    {
        Equipment equipment = await _equipmentRepository.GetById(dto.EquipmentId);
        EquipmentDomainModel equipmentModel = EquipmentService.ParseToModel(equipment);
        Boolean found = false;
        foreach (Inventory item in roomFrom.Inventories)
        {
            if (item.EquipmentId == equipmentModel.Id)
            {
                if (item.Amount < dto.Amount) throw new NotEnoughResourcesForTransfer();
                item.Amount -= dto.Amount;
                found = true;
                break;
            }
        }
        if (!found) throw new NotEnoughResourcesForTransfer();
        
        found = false;
        foreach (Inventory item in roomTo.Inventories)
        {
            if (item.EquipmentId == equipmentModel.Id)
            {
                item.Amount += dto.Amount;
                found = true;
                break;
            }
        }
        if (found) return equipmentModel;

        roomTo.Inventories.Add(CreateNewInventory(roomTo.Id, equipmentModel.Id, dto.Amount));
        return equipmentModel;
    }

    public Inventory CreateNewInventory(decimal roomId, decimal equipmentId, decimal amount)
    {
        Inventory inventory = new Inventory
        {
            EquipmentId = equipmentId,
            Amount = amount,
            IsDeleted = false,
            RoomId = roomId
        };
        return inventory;
    }
    public static EquipmentRequestDomainModel ParseToModel(EquipmentRequest equipmentRequest)
    {
        EquipmentRequestDomainModel equipmentRequestModel = new EquipmentRequestDomainModel 
        {
            Id = equipmentRequest.Id,
            EquipmentId = equipmentRequest.EquipmentId,
            Amount = equipmentRequest.Amount,
            ExecutionTime = equipmentRequest.ExecutionTime,
            IsExecuted = equipmentRequest.IsExecuted
        };
        
        if (equipmentRequest.Equipment != null)
            equipmentRequestModel.Equipment = EquipmentService.ParseToModel(equipmentRequest.Equipment);
        
        return equipmentRequestModel;
    }
    
    public static EquipmentRequest ParseFromModel(EquipmentRequestDomainModel equipmentRequestModel)
    {
        EquipmentRequest equipmentRequest = new EquipmentRequest 
        {
            Id = equipmentRequestModel.Id,
            EquipmentId = equipmentRequestModel.EquipmentId,
            Amount = equipmentRequestModel.Amount,
            ExecutionTime = equipmentRequestModel.ExecutionTime,
            IsExecuted = equipmentRequestModel.IsExecuted
        };
        
        if (equipmentRequestModel.Equipment != null)
            equipmentRequest.Equipment = EquipmentService.ParseFromModel(equipmentRequestModel.Equipment);
        
        return equipmentRequest;
    }
}

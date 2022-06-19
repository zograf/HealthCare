using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class RoomService : IRoomService
{
    private IRoomRepository _roomRepository;
    private IRoomTypeRepository _roomTypeRepository;
    private IExaminationRepository _examinationRepository;
    private IOperationRepository _operationRepository;

    public RoomService(IRoomRepository roomRepository, 
                       IRoomTypeRepository roomTypeRepository,
                       IExaminationRepository examinationRepository, 
                       IOperationRepository operationRepository) 
    {
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
        _examinationRepository = examinationRepository;
        _operationRepository = operationRepository;
    }

    public async Task<IEnumerable<RoomDomainModel>> ReadAll()
    {
        IEnumerable<RoomDomainModel> rooms = await GetAll();
        List<RoomDomainModel> result = new List<RoomDomainModel>();
        foreach (RoomDomainModel item in rooms)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    }

  public async Task<IEnumerable<RoomDomainModel>> GetAll()
    {
        IEnumerable<Room> rooms = await _roomRepository.GetAll();
        if (rooms == null)
            return new List<RoomDomainModel>();
            
        List<RoomDomainModel> results = new List<RoomDomainModel>();
        foreach (Room item in rooms)
            results.Add(ParseToModel(item));
        
        return results;
    }

    public async Task<RoomDomainModel> Create(CURoomDTO dto)
    {
        Room newRoom = new Room();
        newRoom.IsDeleted = false;
        newRoom.RoomName = dto.RoomName;
        newRoom.IsFormed = true;
        RoomType roomType = await _roomTypeRepository.GetById(dto.RoomTypeId);
        if (roomType == null)
            throw new RoomTypeNotFoundException();
        newRoom.RoomType = roomType;
        newRoom.RoomTypeId = roomType.Id;
        _ = _roomRepository.Post(newRoom);
        _roomRepository.Save();

        return ParseToModel(newRoom);
    }

    public async Task<RoomDomainModel> Update(CURoomDTO dto)
    {
        Room room = await _roomRepository.GetRoomById(dto.RoomId);
        room.RoomName = dto.RoomName;
        RoomType roomType = await _roomTypeRepository.GetById(dto.RoomTypeId);
        if (roomType == null)
            throw new RoomTypeNotFoundException();
        room.RoomType = roomType;
        room.RoomTypeId = roomType.Id;
        _ = _roomRepository.Update(room);
        _roomRepository.Save();

        return ParseToModel(room);
    }

    public async Task<RoomDomainModel> Delete(decimal id)
    {
        Room deletedRoom = await _roomRepository.GetRoomById(id);
        deletedRoom.IsDeleted = true;
        _ = _roomRepository.Update(deletedRoom);
        _roomRepository.Save();
        return ParseToModel(deletedRoom);
    }

    private async Task<bool> isRoomAvailableForOperation(decimal id, DateTime startTime, decimal duration)
    {
        bool isRoomAvailable = true;
        IEnumerable<Operation> operations = await _operationRepository.GetAllByRoomId(id);
        foreach (Operation operation in operations)
        {
            double difference = (startTime - operation.StartTime).TotalMinutes;
            if (difference <= (double)operation.Duration && difference >= -(double)duration)
            {
                isRoomAvailable = false;
                break;
            }
        }

        return isRoomAvailable;
    }

    private async Task<bool> isRoomAvailableForExamination(decimal id, DateTime startTime)
    {
        bool isRoomAvailable = true;
        IEnumerable<Examination> examinations = await _examinationRepository.GetAllByRoomId(id);
        foreach (Examination examination in examinations)
        {
            double difference = (startTime - examination.StartTime).TotalMinutes;
            if (difference <= 15 && difference >= -15)
            {
                isRoomAvailable = false;
                break;
            }
        }

        return isRoomAvailable;
    }

    public async Task<decimal> GetAvailableRoomId(DateTime startTime, string roomType, decimal duration = 15)
    {
        IEnumerable<Room> rooms = await _roomRepository.GetAllAppointmentRooms(roomType);
        foreach (Room room in rooms)
        {
            bool roomAvailable = (roomType == "examination") ? await isRoomAvailableForExamination(room.Id, startTime) : await isRoomAvailableForOperation(room.Id, startTime, duration);
            if (roomAvailable)
            {
                return room.Id;
            }
        }
        return -1;
    }

    public static RoomDomainModel ParseToModel(Room room)
    {
        RoomDomainModel roomModel = new RoomDomainModel
        {
            IsDeleted = room.IsDeleted,
            Id = room.Id,
            RoomName = room.RoomName,
            RoomTypeId = room.RoomTypeId,
            IsFormed = room.IsFormed
        };
        
        if(room.RoomType != null)
            roomModel.RoomType = RoomTypeService.ParseToModel(room.RoomType);
        
        roomModel.Inventories = new List<InventoryDomainModel>();
        roomModel.Operations = new List<OperationDomainModel>();
        if (room.Inventories != null) 
            foreach (Inventory inventory in room.Inventories) 
                roomModel.Inventories.Add(InventoryService.ParseToModel(inventory));
        
        if (room.Operations != null) 
            foreach (Operation operation in room.Operations) 
                roomModel.Operations.Add(OperationService.ParseToModel(operation));
        
        return roomModel;
    }
    
    public static Room ParseFromModel(RoomDomainModel roomModel)
    {
        Room room = new Room
        {
            IsDeleted = roomModel.IsDeleted,
            Id = roomModel.Id,
            RoomName = roomModel.RoomName,
            RoomTypeId = roomModel.RoomTypeId,
            IsFormed = roomModel.IsFormed
        };
        
        if(roomModel.RoomType != null)
            room.RoomType = RoomTypeService.ParseFromModel(roomModel.RoomType);
        
        room.Inventories = new List<Inventory>();
        room.Operations = new List<Operation>();
        if (roomModel.Inventories != null) 
            foreach (InventoryDomainModel inventoryModel in roomModel.Inventories) 
                room.Inventories.Add(InventoryService.ParseFromModel(inventoryModel));
        
        if (roomModel.Operations != null) 
            foreach (OperationDomainModel operationModel in roomModel.Operations) 
                room.Operations.Add(OperationService.ParseFromModel(operationModel));
        
        return room;
    }
}
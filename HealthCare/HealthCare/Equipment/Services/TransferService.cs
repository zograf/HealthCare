using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class TransferService : ITransferService
{
    private ITransferRepository _transferRepository;
    private IInventoryRepository _inventoryRepository;

    public TransferService(ITransferRepository transferRepository, 
                           IInventoryRepository inventioryRepository) 
    {
        _transferRepository = transferRepository;
        _inventoryRepository = inventioryRepository;
    }

    public async Task<IEnumerable<TransferDomainModel>> GetAll()
    {
        IEnumerable<Transfer> transfers = await _transferRepository.GetAll();
        if (transfers == null)
            return new List<TransferDomainModel>();

        List<TransferDomainModel> results = new List<TransferDomainModel>();
        TransferDomainModel transferModel;
        foreach (Transfer item in transfers)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<TransferDomainModel> Create(TransferDomainModel transferModel)
    {
        Inventory inRoomInventory = await _inventoryRepository.GetInventoryById(transferModel.RoomIdIn, transferModel.EquipmentId);

        Inventory outRoomInventory = await _inventoryRepository.GetInventoryById(transferModel.RoomIdOut, transferModel.EquipmentId);
        _inventoryRepository.Update(outRoomInventory);

        if(outRoomInventory.Amount < transferModel.Amount)
        {
            throw new NotEnoughResourcesForTransfer();
        }

        if (inRoomInventory == null)
        {
            inRoomInventory = new Inventory
            {
                Amount = 0,
                EquipmentId = transferModel.EquipmentId,
                RoomId = transferModel.RoomIdIn,
                IsDeleted = false
            };               
            _inventoryRepository.Post(inRoomInventory);
        }

        Transfer transfer = new Transfer
        {
            RoomIdIn = transferModel.RoomIdIn,
            RoomIdOut = transferModel.RoomIdOut,
            TransferTime = transferModel.TransferTime,
            Amount = transferModel.Amount,
            EquipmentId = transferModel.EquipmentId,
            Executed = transferModel.Executed
        };
        
        _transferRepository.Post(transfer);
        _inventoryRepository.Save();
        return ParseToModel(transfer);
    }

    public static TransferDomainModel ParseToModel(Transfer transfer)
    {
        TransferDomainModel transferModel = new TransferDomainModel
        {
            Id = transfer.Id,
            RoomIdOut = transfer.RoomIdOut,
            RoomIdIn = transfer.RoomIdIn,
            TransferTime = transfer.TransferTime,
            Amount = transfer.Amount,
            EquipmentId = transfer.EquipmentId,
            Executed = transfer.Executed,
            IsDeleted = transfer.IsDeleted
        };
        
        if (transfer.Equipment != null)
            transferModel.Equipment = EquipmentService.ParseToModel(transfer.Equipment);
        
        return transferModel;
    }
    
    public static Transfer ParseFromModel(TransferDomainModel transferModel)
    {
        Transfer transfer = new Transfer
        {
            Id = transferModel.Id,
            RoomIdOut = transferModel.RoomIdOut,
            RoomIdIn = transferModel.RoomIdIn,
            TransferTime = transferModel.TransferTime,
            Amount = transferModel.Amount,
            EquipmentId = transferModel.EquipmentId,
            Executed = transferModel.Executed,
            IsDeleted = transferModel.IsDeleted
        };
        
        if (transferModel.Equipment != null)
            transfer.Equipment = EquipmentService.ParseFromModel(transferModel.Equipment);
        
        return transfer;
    }

    public async Task<IEnumerable<TransferDomainModel>> DoTransfers()
    {
        IEnumerable<Transfer> transfers = await _transferRepository.GetAll();
        if (transfers == null)
            throw new DataIsNullException();

        List<TransferDomainModel> transfersExecuted = new List<TransferDomainModel>();

        foreach(Transfer transfer in transfers)
        {
            if(transfer.TransferTime < DateTime.UtcNow && !transfer.Executed)
            {
                Inventory roomIn = await _inventoryRepository.GetInventoryById(transfer.RoomIdIn, transfer.EquipmentId);
                Inventory roomOut = await _inventoryRepository.GetInventoryById(transfer.RoomIdOut, transfer.EquipmentId);
                roomIn.Amount += transfer.Amount;
                roomOut.Amount -= transfer.Amount;
                transfer.Executed = true;
                _inventoryRepository.Update(roomIn);
                _inventoryRepository.Update(roomOut);
                _transferRepository.Update(transfer);
                _transferRepository.Save();
                transfersExecuted.Add(ParseToModel(transfer));
            }
        }
        return transfersExecuted;
    }
     
    public async Task<IEnumerable<TransferDomainModel>> ReadAll()
    {
        IEnumerable<TransferDomainModel> transfers = await GetAll();
        List<TransferDomainModel> result = new List<TransferDomainModel>();
        foreach (TransferDomainModel item in transfers)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    } 
}
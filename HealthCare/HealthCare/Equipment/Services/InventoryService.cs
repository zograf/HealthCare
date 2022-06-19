using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class InventoryService : IInventoryService 
{
    private IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository) 
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<IEnumerable<InventoryDomainModel>> GetAll()
    {
        IEnumerable<Inventory> inventories = await _inventoryRepository.GetAll();
        if (inventories == null)
            return new List<InventoryDomainModel>();

        List<InventoryDomainModel> results = new List<InventoryDomainModel>();
        foreach (Inventory item in inventories)
        {
            results.Add(ParseToModel(item));
        }

        return results;

    }

    public async Task<IEnumerable<InventoryDomainModel>> GetDynamicForRoom(decimal roomId)
    {
        IEnumerable<Inventory> inventories = await _inventoryRepository.GetDynamicByRoomId(roomId);
        if (inventories == null)
            return new List<InventoryDomainModel>();

        List<InventoryDomainModel> results = new List<InventoryDomainModel>();
        foreach (Inventory item in inventories)
        {
            results.Add(ParseToModel(item));
        }

        return results;

    }
    public async Task<IEnumerable<InventoryDomainModel>> UpdateRoomInventory(IEnumerable<InventoryDomainModel> inventory)
    {
        foreach (InventoryDomainModel item in inventory)
        {
            Inventory oldInventory = ParseFromModel(item);
            _inventoryRepository.Update(oldInventory);
        }

        _inventoryRepository.Save();

        return inventory;
    }

    public static Inventory ParseFromModel(InventoryDomainModel inventoryModel)
    {
        Inventory inventory = new Inventory
        {
            EquipmentId = inventoryModel.EquipmentId,
            Amount = inventoryModel.Amount,
            IsDeleted = inventoryModel.IsDeleted,
            RoomId = inventoryModel.RoomId
        };

        if (inventoryModel.Equipment != null)
            inventory.Equipment = EquipmentService.ParseFromModel(inventoryModel.Equipment);

        return inventory;
    } 
    public static InventoryDomainModel ParseToModel(Inventory inventory)
    {
        InventoryDomainModel inventoryModel = new InventoryDomainModel
        {
            EquipmentId = inventory.EquipmentId,
            Amount = inventory.Amount,
            IsDeleted = inventory.IsDeleted,
            RoomId = inventory.RoomId
        };

        if (inventory.Equipment != null)
            inventoryModel.Equipment = EquipmentService.ParseToModel(inventory.Equipment);

        return inventoryModel;
    } 
    public async Task<IEnumerable<InventoryDomainModel>> ReadAll()
    {
        IEnumerable<InventoryDomainModel> inventory = await GetAll();
        List<InventoryDomainModel> result = new List<InventoryDomainModel>();
        foreach (InventoryDomainModel item in inventory)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    }

}
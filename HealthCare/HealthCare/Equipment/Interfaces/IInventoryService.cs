using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IInventoryService : IService<InventoryDomainModel> 
{
    public Task<IEnumerable<InventoryDomainModel>> ReadAll();
    public Task<IEnumerable<InventoryDomainModel>> GetDynamicForRoom(decimal roomId);
    public Task<IEnumerable<InventoryDomainModel>> UpdateRoomInventory(IEnumerable<InventoryDomainModel> inventory);
}
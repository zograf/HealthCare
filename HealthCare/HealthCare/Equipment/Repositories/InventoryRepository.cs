using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthCare.Repositories 
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        public Task<Inventory> GetInventoryById(decimal roomId, decimal equipmentId);
        public Inventory Update(Inventory updatedInventory);
        public Inventory Post(Inventory newInventory);
        public Task<IEnumerable<Inventory>> Get(Room splitRoom);
        public Task<Inventory> Get(Room storageRoom, Equipment equipment);
        public Task<IEnumerable<Inventory>> GetDynamicByRoomId(decimal roomId);
    }
    public class InventioryRepository : IInventoryRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public InventioryRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Inventory>> GetAll() 
        {
            return await _healthCareContext.Inventories
                .Include(x => x.Equipment)
                .ToListAsync();
        }
        public async Task<IEnumerable<Inventory>> Get(Room splitRoom)
        {
            return await _healthCareContext.Inventories.
                Where(i => i.RoomId == splitRoom.Id).
                ToListAsync();
        }

        public async Task<Inventory> Get(Room room, Equipment equipment)
        {
            return await _healthCareContext.Inventories.
                Where(i => i.EquipmentId == equipment.Id && i.RoomId == room.Id).
                FirstOrDefaultAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
        public async Task<Inventory> GetInventoryById(decimal roomId, decimal equipmentId)
        {
            return await _healthCareContext.Inventories.FindAsync(roomId, equipmentId);
        }

        public Inventory Update(Inventory updatedInventory)
        {
            EntityEntry<Inventory> updatedEntry = _healthCareContext.Attach(updatedInventory);
            _healthCareContext.Entry(updatedInventory).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public Inventory Post(Inventory newInventory)
        {
            EntityEntry<Inventory> result = _healthCareContext.Inventories.Add(newInventory);
            return result.Entity;
        }

        public async Task<IEnumerable<Inventory>> GetDynamicByRoomId(decimal roomId)
        {
            return await _healthCareContext.Inventories
                .Where(x => x.RoomId == roomId)
                .Where(x => x.Equipment.IsDynamic == true)
                .Include(x => x.Equipment)
                .ThenInclude(x => x.EquipmentType)
                .ToListAsync();
        }
    }
}

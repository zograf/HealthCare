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
    public interface IRoomRepository : IRepository<Room> 
    {
        public Task<IEnumerable<Room>> GetAllExaminationRooms();

        public Task<IEnumerable<Room>> GetAllAppointmentRooms(string roomPurpose);

        public Room Post(Room r);
        public Task<Room> GetRoomById(decimal id);
        public Room Update(Room r);
        Task<Room> GetRoomByName(string v);
    }

    public class RoomRepository : IRoomRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public RoomRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Room>> GetAll() 
        {
            return await _healthCareContext.Rooms
                .Include(x => x.RoomType)
                .Include(x => x.Inventories).ThenInclude(x => x.Equipment).ThenInclude(x => x.EquipmentType)
                .Include(x => x.Operations)
                .ToListAsync();
        }
        public async Task<Room> GetRoomByName(string name)
        {
            return await _healthCareContext.Rooms
                .Include(x => x.RoomType)
                .Include(x => x.Inventories).ThenInclude(x => x.Equipment).ThenInclude(x => x.EquipmentType)
                .Include(x => x.Operations)
                .FirstOrDefaultAsync(r => r.RoomName == name);
        }

        // Argument roomPurpose differantiates if the fetched rooms should be rooms for operation/examination
        public async Task<IEnumerable<Room>> GetAllAppointmentRooms(string roomPurpose) 
        {
            return await _healthCareContext.Rooms
                .Include(x => x.RoomType)
                .Where(x => x.RoomType.Purpose == roomPurpose)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllExaminationRooms()
        {
            return await _healthCareContext.Rooms
                .Include(x => x.RoomType)
                .Where(x => x.RoomType.Purpose == "examination")
                .ToListAsync();
        }

        public Room Post(Room newRoom)
        {
            EntityEntry<Room> result = _healthCareContext.Add(newRoom);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

        public async Task<Room> GetRoomById(decimal id)
        {
            return await _healthCareContext.Rooms
                .Include(x => x.RoomType)
                .Include(x => x.Inventories).ThenInclude(x => x.Equipment).ThenInclude(x => x.EquipmentType)
                .Include(x => x.Operations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Room Update(Room updatedRoom)
        {
            EntityEntry<Room> updatedEntry = _healthCareContext.Attach(updatedRoom);
            _healthCareContext.Entry(updatedRoom).State = EntityState.Modified;
            return updatedEntry.Entity;
        }
    }
}

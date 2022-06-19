using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthCare.Repositories {
    public interface IEquipmentRequestRepository : IRepository<EquipmentRequest>
    {
        public EquipmentRequest Update(EquipmentRequest equipmentRequest);
        public EquipmentRequest Post(EquipmentRequest equipmentRequest);
    }

    public class EquipmentRequestRepository : IEquipmentRequestRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public EquipmentRequestRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<EquipmentRequest>> GetAll() 
        {
            return await _healthCareContext.EquipmentRequests
                .Include(x => x.Equipment)
                .ToListAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
        
        public EquipmentRequest Post(EquipmentRequest equipmentRequest)
        {
            EntityEntry<EquipmentRequest> result = _healthCareContext.EquipmentRequests.Add(equipmentRequest);
            return result.Entity;
        }
        
        public EquipmentRequest Update(EquipmentRequest equipmentRequest)
        {
            EntityEntry<EquipmentRequest> updatedEntry = _healthCareContext.EquipmentRequests.Attach(equipmentRequest);
            _healthCareContext.Entry(equipmentRequest).State = EntityState.Modified;
            return updatedEntry.Entity;
        }
    }
}

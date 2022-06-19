using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Repositories {
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        public Task<Equipment> GetById(decimal equipmentId);
        public Task<Equipment> GetByName(string name);
    }

    public class EquipmentRepository : IEquipmentRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public EquipmentRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Equipment>> GetAll() 
        {
            return await _healthCareContext.Equipments.Include(x => x.EquipmentType).ToListAsync();
        }

        public async Task<Equipment> GetById(decimal equipmentId)
        {
            return await _healthCareContext.Equipments.FindAsync(equipmentId);
        }
        
        public async Task<Equipment> GetByName(string name)
        {
            return await _healthCareContext.Equipments
                .Include(x=>x.EquipmentType)
                .Where(x=>x.Name == name)
                .FirstOrDefaultAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

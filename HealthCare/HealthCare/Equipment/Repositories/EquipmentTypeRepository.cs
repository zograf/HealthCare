using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Repositories 
{
    public interface IEquipmentTypeRepository : IRepository<EquipmentType> 
    {

    }
    public class EquipmentTypeRepository : IEquipmentTypeRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public EquipmentTypeRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<EquipmentType>> GetAll() 
        {
            return await _healthCareContext.EquipmentTypes.ToListAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

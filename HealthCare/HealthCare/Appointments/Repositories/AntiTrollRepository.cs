using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthCare.Repositories 
{

    public interface IAntiTrollRepository : IRepository<AntiTroll> 
    {
        public Task<IEnumerable<AntiTroll>> GetByPatientId(decimal patientId);
        public AntiTroll Post(AntiTroll antiTroll);
    }
    public class AntiTrollRepository : IAntiTrollRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public AntiTrollRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<AntiTroll>> GetAll() 
        {
            return await _healthCareContext.AntiTrolls.ToListAsync();
        }

        public async Task<IEnumerable<AntiTroll>> GetByPatientId(decimal patientId) 
        {
            return await _healthCareContext.AntiTrolls.Where(x => x.PatientId == patientId).ToListAsync();
        }

        public AntiTroll Post(AntiTroll antiTroll) 
        {
            EntityEntry<AntiTroll> result = _healthCareContext.AntiTrolls.Add(antiTroll);
            return result.Entity;
        }

        public void Save() 
        {
            _healthCareContext.SaveChanges();
        }
    }
}

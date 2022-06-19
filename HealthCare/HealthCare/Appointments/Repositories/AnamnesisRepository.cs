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
    public interface IAnamnesisRepository : IRepository<Anamnesis> 
    {
        public Anamnesis Post(Anamnesis anamensis);
        public Anamnesis Update(Anamnesis anamnesis);
    }
    public class AnamnesisRepository : IAnamnesisRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public AnamnesisRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Anamnesis>> GetAll() 
        {
            return await _healthCareContext.Anamneses.ToListAsync();
        }

        public Anamnesis Post(Anamnesis anamensis)
        {
            EntityEntry<Anamnesis> result = _healthCareContext.Anamneses.Add(anamensis);
            return result.Entity;
        }

        public Anamnesis Update(Anamnesis anamnesis)
        {
            EntityEntry<Anamnesis> updatedEntry = _healthCareContext.Anamneses.Attach(anamnesis);
            _healthCareContext.Entry(anamnesis).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }

}

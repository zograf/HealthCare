using HealthCare.Data.Entities;
using HealthCare.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthCare.Repositories
{
    public interface IAllergyRepository : IRepository<Allergy>
    {
        public Allergy Post(Allergy allergy);
        public Allergy Update(Allergy allergy);
        public Task<IEnumerable<Allergy>> GetAllByPatientId(decimal patientId);

        public Task<Allergy> GetById(decimal patientId, decimal ingredientId);
    }
    public class AllergyRepository : IAllergyRepository
    {
        private readonly HealthCareContext _healthCareContext;
        public AllergyRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<Allergy>> GetAll()
        {
            return await _healthCareContext.Allergies
                        .Include(x => x.Ingredient)
                        .Where(x => x.IsDeleted == false)
                        .ToListAsync();
        }

        public Allergy Post(Allergy allergy)
        {
            EntityEntry<Allergy> result = _healthCareContext.Allergies.Add(allergy);
            return result.Entity;
        }

        public Allergy Update(Allergy allergy)
        {
            EntityEntry<Allergy> updatedEntry = _healthCareContext.Allergies.Attach(allergy);
            _healthCareContext.Entry(allergy).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

        public async Task<IEnumerable<Allergy>> GetAllByPatientId(decimal patientId)
        {
            return await _healthCareContext.Allergies
                    .Where(x => x.PatientId == patientId)
                    .Include(x => x.Ingredient)
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();

        }

        public async Task<Allergy> GetById(decimal patientId, decimal ingredientId)
        {
            return await _healthCareContext.Allergies
                         .Include(x => x.Ingredient)
                         .Where(x => x.IngredientId == ingredientId && x.PatientId == patientId)
                         .Where(x => x.IsDeleted == false)
                         .FirstOrDefaultAsync();
        }
    }
}

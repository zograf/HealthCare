using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repositories
{
    public interface IDrugRepository : IRepository<Drug>
    {
        public Task<Drug> GetById(decimal id);
        Drug Post(Drug drug);
        Drug Update(Drug drug);
    }

    public class DrugRepository : IDrugRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public DrugRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<Drug>> GetAll()
        {
            return await _healthCareContext.Drugs
                .Include(x => x.DrugIngredients).ThenInclude(x => x.Ingredient)
                .ToListAsync();
        }

        public async Task<Drug> GetById(decimal id)
        {
            return await _healthCareContext.Drugs
                .Include(x => x.DrugIngredients).ThenInclude(x => x.Ingredient)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public Drug Post(Drug drug)
        {
            EntityEntry<Drug> result = _healthCareContext.Drugs.Add(drug);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

        public Drug Update(Drug drug)
        {
            EntityEntry<Drug> updatedEntry = _healthCareContext.Drugs.Attach(drug);
            _healthCareContext.Entry(drug).State = EntityState.Modified;
            return updatedEntry.Entity;
        }
    }
}

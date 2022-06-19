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
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        public Ingredient Post(Ingredient ingredient);
        public Ingredient Update(Ingredient ingredient);
        public Task<Ingredient> Get(decimal id);
        public Task<IEnumerable<Ingredient>> GetAll();
        public void Save();
    }
    public class IngredientRepository : IIngredientRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public IngredientRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<Ingredient> Get(decimal id)
        {
            return await _healthCareContext.Ingredients.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetAll()
        {
            return await _healthCareContext.Ingredients.Include(x => x.DrugIngredients).ToListAsync();
        }

        public Ingredient Post(Ingredient ingredient)
        {
            EntityEntry<Ingredient> result = _healthCareContext.Ingredients.Add(ingredient);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

        public Ingredient Update(Ingredient ingredient)
        {
            EntityEntry<Ingredient> updatedEntry = _healthCareContext.Ingredients.Attach(ingredient);
            _healthCareContext.Entry(ingredient).State = EntityState.Modified;
            return updatedEntry.Entity;
        }
    }
}

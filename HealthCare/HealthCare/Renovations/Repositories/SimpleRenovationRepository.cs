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
    public interface ISimpleRenovationRepository : IRepository<SimpleRenovation>
    {
        public SimpleRenovation Post(SimpleRenovation r);
    }
    public class SimpleRenovationRepository : ISimpleRenovationRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public SimpleRenovationRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<SimpleRenovation>> GetAll()
        {
            return await _healthCareContext.SimpleRenovations.ToListAsync();
        }

        public SimpleRenovation Post(SimpleRenovation renovation)
        {
            EntityEntry<SimpleRenovation> result = _healthCareContext.SimpleRenovations.Add(renovation);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

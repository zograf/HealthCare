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
    public interface ISplitRenovationRepository : IRepository<SplitRenovation>
    {
        public SplitRenovation Post(SplitRenovation r);
    }
    public class SplitRenovationRepository : ISplitRenovationRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public SplitRenovationRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<SplitRenovation>> GetAll()
        {
            return await _healthCareContext.SplitRenovations.ToListAsync();
        }

        public SplitRenovation Post(SplitRenovation renovation)
        {
            EntityEntry<SplitRenovation> result = _healthCareContext.SplitRenovations.Add(renovation);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

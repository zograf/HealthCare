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
    public interface IJoinRenovationRepository : IRepository<JoinRenovation>
    {
        public JoinRenovation Post(JoinRenovation r);
    }
    public class JoinRenovationRepository : IJoinRenovationRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public JoinRenovationRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<JoinRenovation>> GetAll()
        {
            return await _healthCareContext.JoinRenovations.ToListAsync();
        }

        public JoinRenovation Post(JoinRenovation renovation)
        {
            EntityEntry<JoinRenovation> result = _healthCareContext.JoinRenovations.Add(renovation);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

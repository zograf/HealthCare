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
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        public Prescription Post(Prescription prescription);
    }
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public PrescriptionRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<Prescription>> GetAll()
        {
            return await _healthCareContext.Prescriptions
                .Include(x => x.Drug)
                .ToListAsync();
        }

        public Prescription Post(Prescription prescription)
        {
            EntityEntry<Prescription> result = _healthCareContext.Prescriptions.Add(prescription);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

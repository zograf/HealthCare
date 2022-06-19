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
    public interface IDaysOffRequestRepository : IRepository<DaysOffRequest>
    {
        public DaysOffRequest Post(DaysOffRequest daysOffRequest);
        public Task<DaysOffRequest> GetById(decimal id);

        public DaysOffRequest Update(DaysOffRequest daysOff);
        Task<IEnumerable<DaysOffRequest>> GetAllByDoctorId(decimal id);
    }

    public class DaysOffRequesRepository : IDaysOffRequestRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public DaysOffRequesRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        public async Task<IEnumerable<DaysOffRequest>> GetAll()
        {
            return await _healthCareContext.DaysOffRequests.ToListAsync();
        }

        public async Task<IEnumerable<DaysOffRequest>> GetAllByDoctorId(decimal id)
        {
            return await _healthCareContext.DaysOffRequests.Where(x => x.DoctorId == id).ToListAsync();
        }

        public async Task<DaysOffRequest> GetById(decimal id)
        {
            return await _healthCareContext.DaysOffRequests
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public DaysOffRequest Post(DaysOffRequest daysOffRequest)
        {
            EntityEntry<DaysOffRequest> result = _healthCareContext.DaysOffRequests.Add(daysOffRequest);
            return result.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
        
        public DaysOffRequest Update(DaysOffRequest daysOff)
        {
            EntityEntry<DaysOffRequest> updatedEntry = _healthCareContext.DaysOffRequests.Attach(daysOff);
            _healthCareContext.Entry(daysOff).State = EntityState.Modified;
            return updatedEntry.Entity;
        }
    }
}

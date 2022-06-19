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

    public interface ICredentialsRepository : IRepository<Credentials> 
    {
        public Credentials Post(Credentials credentials);
        public Task<Credentials> GetCredentialsById(decimal id);
        public Task<Credentials> GetCredentialsByPatientId(decimal id);
        public Credentials Update(Credentials credentials);
        public Task<Credentials> GetCredentialsByDoctorId(decimal id);
    }
    public class CredentialsRepository : ICredentialsRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public CredentialsRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Credentials>> GetAll()
        {
            //return await _healthCareContext.Credentials.Include(x=> x.Doctor).Include(x => x.Secretary).Include(x => x.Patient).Include(x => x.Manager).ToListAsync();
            return await _healthCareContext.Credentials
                .Include(x => x.UserRole)
                .ToListAsync();
        }

        public async Task<Credentials> GetCredentialsById(decimal id) {
            return await _healthCareContext.Credentials.FirstAsync(x => x.Id == id);
        }

        public async Task<Credentials> GetCredentialsByPatientId(decimal id) {
            return await _healthCareContext.Credentials.FirstAsync(x => x.PatientId == id);
        }
        public async Task<Credentials> GetCredentialsByDoctorId(decimal id) {
            return await _healthCareContext.Credentials.FirstAsync(x => x.DoctorId == id);
        }
        public Credentials Post(Credentials credentials) {
            EntityEntry<Credentials> result = _healthCareContext.Add(credentials);
            return result.Entity;
        }

        public Credentials Update(Credentials credentials) {
            EntityEntry<Credentials> updatedEntry = _healthCareContext.Credentials.Attach(credentials);
            _healthCareContext.Entry(credentials).State = EntityState.Modified;
            return updatedEntry.Entity;
        }
        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

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
    public interface IMedicalRecordRepository : IRepository<MedicalRecord> 
    {
        public MedicalRecord Post(MedicalRecord medicalRecord);
        public Task<MedicalRecord> GetByPatientId(decimal patientId);
        public MedicalRecord Update(MedicalRecord medicalRecord);

    }
    public class MedicalRecordRepository : IMedicalRecordRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public MedicalRecordRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<MedicalRecord>> GetAll() 
        {
            return await _healthCareContext.MedicalRecords.Include(x => x.ReferralLetters).ToListAsync();
        }

        public async Task<MedicalRecord> GetByPatientId(decimal patientId) 
        {
            return await _healthCareContext.MedicalRecords
                .Include(x => x.ReferralLetters).ThenInclude(x => x.Specialization)
                .Include(x => x.AllergiesList)
                .Where(x => x.PatientId == patientId)
                .FirstOrDefaultAsync();
        }

        public MedicalRecord Post(MedicalRecord medicalRecord) 
        {
            EntityEntry<MedicalRecord> result = _healthCareContext.Add(medicalRecord);
            return result.Entity;
        }

        public MedicalRecord Update(MedicalRecord medicalRecord) 
        {
            EntityEntry<MedicalRecord> updatedEntry = _healthCareContext.MedicalRecords.Attach(medicalRecord);
            _healthCareContext.Entry(medicalRecord).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

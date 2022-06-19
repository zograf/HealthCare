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
    public interface IPatientRepository : IRepository<Patient> 
    {
        public Patient Post(Patient patient);
        public Patient Update(Patient patient);
        public Patient Delete(Patient patient);
        public Task<Patient> GetPatientById(decimal id);
        public Task<Patient> GetWithMedicalRecord(decimal id);
    }
    public class PatientRepository : IPatientRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public PatientRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Patient>> GetAll() 
        {
            return await _healthCareContext.Patients
                .Include(x => x.Credentials)
                .Include(x => x.MedicalRecord)
                .Include(x => x.Operations)
                .Include(x => x.Examinations).ThenInclude(x => x.Anamnesis)
                //.Include(x => x.Examinations).ThenInclude(x => x.ExaminationApproval)
                .ToListAsync();
        }

        public Patient Delete(Patient patient)
        {
            Patient deletedPatient = Update(patient);
            return deletedPatient;
        }

        public async Task<Patient> GetPatientById(decimal id)
        {
            return await _healthCareContext.Patients
                .Include(x => x.Credentials)
                .Include(x => x.MedicalRecord).ThenInclude(x => x.ReferralLetters)
                .Include(x => x.Operations)
                .Include(x => x.Examinations).ThenInclude(x => x.Anamnesis)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Patient> GetWithMedicalRecord(decimal id)
        {
            return await _healthCareContext.Patients
                .Where(x => x.Id == id)
                .Include(x => x.MedicalRecord)
                .FirstOrDefaultAsync();
        }

        public Patient Post(Patient patient)
        {
            EntityEntry<Patient> result = _healthCareContext.Patients.Add(patient);
            return result.Entity;
        }

        public Patient Update(Patient patient)
        {
            EntityEntry<Patient> updatedEntry = _healthCareContext.Patients.Attach(patient);
            _healthCareContext.Entry(patient).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}

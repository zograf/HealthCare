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
    public interface IDoctorRepository : IRepository<Doctor> 
    {
        public Doctor Post(Doctor doctor);
        public Doctor Update(Doctor doctor);
        public Doctor Delete(Doctor doctor);
        public Task<Doctor> GetById(decimal id);
        public Task<IEnumerable<Doctor>> GetBySpecialization(decimal id);
    }
    public class DoctorRepository : IDoctorRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public DoctorRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Doctor>> GetAll() 
        {
            return await _healthCareContext.Doctors
                .Include(x => x.Credentials).ThenInclude(x => x.UserRole)
                .Include(x => x.Specialization)
                .Include(x => x.Examinations).ThenInclude(x => x.Anamnesis)
                .Include(x => x.Operations)
                .ToListAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

        public Doctor Delete(Doctor doctor)
        {
            Doctor deletedDoctor = Update(doctor);
            return deletedDoctor;
        }

        public async Task<Doctor> GetById(decimal id)
        {
            return await _healthCareContext.Doctors
                .Include(x => x.Credentials).ThenInclude(x => x.UserRole)
                .Include(x => x.Specialization)
                .Include(x => x.Examinations).ThenInclude(x => x.Anamnesis)
                .Include(x => x.Operations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Doctor Post(Doctor doctor)
        {
            EntityEntry<Doctor> result = _healthCareContext.Doctors.Add(doctor);
            return result.Entity;
        }

        public Doctor Update(Doctor doctor)
        {
            EntityEntry<Doctor> updatedEntry = _healthCareContext.Doctors.Attach(doctor);
            _healthCareContext.Entry(doctor).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialization(decimal id)
        {
            return await _healthCareContext.Doctors
               .Include(x => x.Credentials).ThenInclude(x => x.UserRole)
               .Include(x => x.Specialization)
               .Include(x => x.Examinations).ThenInclude(x => x.Anamnesis)
               .Include(x => x.Operations)
               .Where(x => x.IsDeleted == false)
               .Where(x => x.SpecializationId == id)
               .ToListAsync();
        }
    }
}

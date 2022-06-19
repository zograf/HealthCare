using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthCare.Repositories
{
    public interface IExaminationRepository : IRepository<Examination>
    {
        public Examination Post(Examination examination);
        public Task<IEnumerable<Examination>> GetAllByPatientId(decimal id);
        public Task<IEnumerable<Examination>> GetByPatientId(decimal id);
        public Task<IEnumerable<Examination>> GetAllByDoctorId(decimal id);
        public Task<IEnumerable<Examination>> GetAllByDoctorId(decimal id, DateTime date, bool threeDays);
        public Task<IEnumerable<Examination>> GetAllByRoomId(decimal id);
        public Task<Examination> GetByParams(decimal doctorId, decimal roomId, decimal patientId, DateTime startTime);
        public Task<Examination> GetByParams(decimal doctorId, decimal patientId, DateTime startTime);
        public Examination Update(Examination examination);
        public Task<Examination> GetExamination(decimal id);
        public Task<Examination> GetExaminationWithoutAnamnesis(decimal id);
        public Task<Examination> GetByDoctorPatientDate(decimal doctorId, decimal patientId, DateTime date);
    }
    public class ExaminationRepository : IExaminationRepository
    {
        private readonly HealthCareContext _healthCareContext;

        public ExaminationRepository(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Examination>> GetAll()
        {
            return await _healthCareContext.Examinations.Include(x => x.Anamnesis).ToListAsync();
        }

        public async Task<IEnumerable<Examination>> GetAllByPatientId(decimal id)
        {
            return await _healthCareContext.Examinations
                .Where(x => x.PatientId == id)
                .Include(x => x.Anamnesis)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Examination>> GetByPatientId(decimal id)
        {
            return await _healthCareContext.Examinations
                .Where(x => x.PatientId == id)
                .Include(x => x.Anamnesis)
                .ToListAsync();
        }

        public async Task<IEnumerable<Examination>> GetAllByDoctorId(decimal id)
        {
            return await _healthCareContext.Examinations
                .Where(x => x.DoctorId == id)
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Anamnesis)
                .ToListAsync();
        }

        public async Task<IEnumerable<Examination>> GetAllByDoctorId(decimal id, DateTime date, bool threeDays)
        {
            if (threeDays)
                return await _healthCareContext.Examinations
                    .Where(x => x.DoctorId == id)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => DateTime.Compare(x.StartTime, date) >= 0 && DateTime.Compare(x.StartTime, date.AddDays(14)) <= 0)
                    //.Where(x => DateTime.Compare(x.StartTime.Date, date.Date) >= 0 && DateTime.Compare(x.StartTime.Date, date.Date.AddDays(3)) <= 0)
                    .ToListAsync();

            return await _healthCareContext.Examinations
                    .Where(x => x.DoctorId == id)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => DateTime.Compare(x.StartTime.Date, date.Date) == 0)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Examination>> GetAllByRoomId(decimal id)
        {
            return await _healthCareContext.Examinations
                .Where(x => x.RoomId == id)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Examination> GetByParams(decimal doctorId, decimal roomId, decimal patientId, DateTime startTime) {
            return await _healthCareContext.Examinations
                .Where(x => x.RoomId == roomId)
                .Where(x => x.DoctorId == doctorId)
                .Where(x => x.PatientId == patientId)
                .Where(x => x.StartTime == startTime)
                .FirstOrDefaultAsync();
        }

        public Examination Update(Examination examination)
        {
            EntityEntry<Examination> updatedEntry = _healthCareContext.Examinations.Attach(examination);
            _healthCareContext.Entry(examination).State = EntityState.Modified;
            return updatedEntry.Entity;
        }
        public async Task<Examination> GetExamination(decimal id)
        {
            return await _healthCareContext.Examinations
                .Include(x => x.Anamnesis)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Examination> GetExaminationWithoutAnamnesis(decimal id)
        {
            return await _healthCareContext.Examinations
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public Examination Post(Examination examination)
        {
            EntityEntry<Examination> result = _healthCareContext.Examinations.Add(examination);
            return result.Entity;
        }
        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
        public async Task<Examination> GetByDoctorPatientDate(decimal doctorId, decimal patientId, DateTime date)
        {
            return await _healthCareContext.Examinations
                .Include(x => x.Anamnesis)
                .Where(x => x.DoctorId == doctorId && x.PatientId == patientId && x.StartTime == date)
                .FirstOrDefaultAsync();
        }

        public async Task<Examination> GetByParams(decimal doctorId, decimal patientId, DateTime startTime)
        {
            return await _healthCareContext.Examinations
                .Where(x => x.DoctorId == doctorId)
                .Where(x => x.PatientId == patientId)
                .Where(x => x.StartTime == startTime)
                .FirstOrDefaultAsync();
        }
    }
}

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
    public interface IOperationRepository : IRepository<Operation> 
    {
        public Task<Operation> GetById(decimal id);
        public Task<Operation> GetByParams(decimal patientId, decimal doctorId, decimal roomId, DateTime startTime);
        public Task<IEnumerable<Operation>> GetAllByDoctorId(decimal id);
        public Task<IEnumerable<Operation>> GetAllByDoctorId(decimal id, DateTime date);
        public Task<IEnumerable<Operation>> GetAllByPatientId(decimal id);
        public Task<IEnumerable<Operation>> GetAllByRoomId(decimal id);
        public Operation Post(Operation operation);
        public Operation Update(Operation operation);
        public Task<Operation> GetByDoctorPatientDate(decimal doctorId, decimal patientId, DateTime date);
        public Task<Operation> GetByParams(decimal doctorId, decimal patientId, DateTime startTime);
    }
    public class OperationRepository : IOperationRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public OperationRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Operation>> GetAll() 
        {
            return await _healthCareContext.Operations.ToListAsync();
        }

        public async Task<Operation> GetById(decimal id) 
        {
            return await _healthCareContext.Operations
                .Where(x => x.Id == id)
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Operation>> GetAllByDoctorId(decimal id) 
        {
            return await _healthCareContext.Operations
                .Where(x => x.DoctorId == id)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Operation>> GetAllByDoctorId(decimal id, DateTime date)
        {
            return await _healthCareContext.Operations
                .Where(x => x.DoctorId == id)
                .Where(x => x.IsDeleted == false)
                .Where(x => x.StartTime >= date && x.StartTime <= date.AddDays(14))
                //.Where(x => x.StartTime.Date >= date.Date && x.StartTime.Date <= date.Date.AddDays(3))
                .ToListAsync();
        }

        public async Task<IEnumerable<Operation>> GetAllByPatientId(decimal id)
        {
            return await _healthCareContext.Operations
                .Where(x => x.PatientId == id)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Operation>> GetAllByRoomId(decimal id)
        {
            return await _healthCareContext.Operations
                .Where(x => x.RoomId == id)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Operation> GetByParams(decimal patientId, decimal doctorId, decimal roomId, DateTime startTime)
        {
            return await _healthCareContext.Operations
                .Where(x => x.RoomId == roomId)
                .Where(x => x.PatientId == patientId)
                .Where(x => x.DoctorId == doctorId)
                .Where(x => x.StartTime == startTime)
                .FirstOrDefaultAsync();
        }

        public Operation Post(Operation operation)
        {
            EntityEntry<Operation> result = _healthCareContext.Operations.Add(operation);
            return result.Entity;
        }

        public Operation Update(Operation operation)
        {
            EntityEntry<Operation> updatedEntry = _healthCareContext.Operations.Attach(operation);
            _healthCareContext.Entry(operation).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
        
        public async Task<Operation> GetByDoctorPatientDate(decimal doctorId, decimal patientId, DateTime date)
        {
            return await _healthCareContext.Operations
                .Where(x => x.DoctorId == doctorId && x.PatientId == patientId && x.StartTime == date)
                .FirstOrDefaultAsync();
        }
        
        public async Task<Operation> GetByParams(decimal doctorId, decimal patientId, DateTime startTime)
        {
            return await _healthCareContext.Operations
                .Where(x => x.DoctorId == doctorId)
                .Where(x => x.PatientId == patientId)
                .Where(x => x.StartTime == startTime)
                .FirstOrDefaultAsync();
        }
    }
}

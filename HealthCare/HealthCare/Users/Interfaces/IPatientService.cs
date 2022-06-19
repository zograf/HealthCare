using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace HealthCare.Domain.Interfaces;

public interface IPatientService : IService<PatientDomainModel>
{
    public Task<PatientDomainModel> Create(CUPatientDTO dto);
    public Task<PatientDomainModel> Update(CUPatientDTO dto);
    public Task<PatientDomainModel> Delete(decimal id);
    public Task<PatientDomainModel> Block(decimal id);
    public Task<PatientDomainModel> Unblock(decimal id);
    public Task<IEnumerable<PatientDomainModel>> GetBlockedPatients();
    public Task<PatientDomainModel> GetWithMedicalRecord(decimal id);
    public Task<IEnumerable<PatientDomainModel>> ReadAll();
    public Task<IEnumerable<KeyValuePair<DateTime, DateTime>>> GetSchedule(decimal id);
    public Task<PatientDomainModel> UpdateNotificationOffset(NotificationOffsetDTO dto);
    public Task<bool> IsPatientBlocked(decimal patientId);

}
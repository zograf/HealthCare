using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IMedicalRecordService : IService<MedicalRecordDomainModel> 
{
    public Task<MedicalRecordDomainModel> GetForPatient(decimal id);
    public Task<MedicalRecordDomainModel> Update(CUMedicalRecordDTO dto);
    public Task<IEnumerable<MedicalRecordDomainModel>> ReadAll();
}
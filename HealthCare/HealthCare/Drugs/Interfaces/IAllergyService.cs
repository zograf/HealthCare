using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IAllergyService : IService<AllergyDomainModel>
{
    Task<IEnumerable<AllergyDomainModel>> GetAllForPatient(decimal patientId);

    public Task<AllergyDomainModel> Create(AllergyDTO dto);

    public Task<AllergyDomainModel> Delete(AllergyDTO dto);
}
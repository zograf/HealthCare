using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IDoctorService : IService<DoctorDomainModel>
{
    public Task<IEnumerable<DoctorDomainModel>> ReadAll();
    public Task<DoctorDomainModel> GetById(decimal id);
    public Task<IEnumerable<DoctorDomainModel>> GetAllBySpecialization(decimal id);
    public Task<IEnumerable<KeyValuePair<DateTime, DateTime>>> GetAvailableSchedule(decimal doctorId, decimal duration = 15);
    public Task<IEnumerable<KeyValuePair<DateTime, DateTime>>> GetBusySchedule(decimal doctorId);
    public Task<IEnumerable<DoctorDomainModel>> Search(SearchDoctorsDTO dto);
}
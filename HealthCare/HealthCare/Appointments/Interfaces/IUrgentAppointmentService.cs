using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IUrgentAppointmentService : IService<AppointmentDomainModel>
{
    public Task<Boolean> CreateUrgent(CreateUrgentAppointmentDTO dto);

    public Task<IEnumerable<IEnumerable<RescheduleDTO>>> FindFiveAppointments(CreateUrgentAppointmentDTO dto);

    public Task<Boolean> AppointUrgent(List<RescheduleDTO> dto);
}
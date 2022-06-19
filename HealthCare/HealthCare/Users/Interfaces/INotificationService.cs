using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface INotificationService : IService<NotificationDomainModel>
{
    public Task<NotificationDomainModel> Send(SendNotificationDTO dto);
}

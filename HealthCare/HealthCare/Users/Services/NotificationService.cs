using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class NotificationService : INotificationService
{
    private INotificationRepository _notificationRepository;
    private ICredentialsRepository _credentialsRepository;

    public NotificationService(INotificationRepository notificationRepository, 
        ICredentialsRepository credentialsRepository)
    {
        _notificationRepository = notificationRepository;
        _credentialsRepository = credentialsRepository;
    }

    public async Task<IEnumerable<NotificationDomainModel>> GetAll()
    {
        IEnumerable<Notification> data = await _notificationRepository.GetAll();
        if (data == null)
            throw new DataIsNullException();

        List<NotificationDomainModel> results = new List<NotificationDomainModel>();
        foreach (Notification notification in data)
            results.Add(ParseToModel(notification));

        return results;
    }

    public async Task<IEnumerable<NotificationDomainModel>> ReadAll()
    {
        return await GetAll();
    }


    public static NotificationDomainModel ParseToModel(Notification notification)
    {
        NotificationDomainModel notificationModel = new NotificationDomainModel
        {
            Id = notification.Id,
            Content = notification.Content,
            CredentialsId = notification.CredentialsId,
            IsSeen = notification.IsSeen,
            Title = notification.Title
        };

        return notificationModel;
    }
    
    public static Notification ParseFromModel(NotificationDomainModel notificationModel)
    {
        Notification notification = new Notification
        {
            Id = notificationModel.Id,
            Content = notificationModel.Content,
            CredentialsId = notificationModel.CredentialsId,
            IsSeen = notificationModel.IsSeen,
            Title = notificationModel.Title
        };

        return notification;
    }

    public async Task<NotificationDomainModel> Send(SendNotificationDTO dto)
    {
        Credentials credentials;
        if (dto.IsPatient)
            credentials = await _credentialsRepository.GetCredentialsByPatientId(dto.PersonId);
        else
            credentials = await _credentialsRepository.GetCredentialsByDoctorId(dto.PersonId);

        NotificationDomainModel notificationModel = new NotificationDomainModel
        {
            Content = dto.Content.Value,
            Title = dto.Content.Key,
            CredentialsId = credentials.Id,
            IsSeen = false
        };
        _ = _notificationRepository.Post(ParseFromModel(notificationModel));
        _notificationRepository.Save();
        return notificationModel;
    }

}

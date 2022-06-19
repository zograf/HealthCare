
using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HealthCare.Repositories 
{
    public interface INotificationRepository: IRepository<Notification>
    {
        public Notification Post(Notification notification);
        public Notification Update(Notification notification);
    }
    public class NotificationRepository : INotificationRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public NotificationRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Notification>> GetAll() 
        {
            return await _healthCareContext.Notifications.ToListAsync();
        }

        public Notification Post(Notification notification)
        {
            EntityEntry<Notification> result = _healthCareContext.Notifications.Add(notification);
            return result.Entity;
        }

        public Notification Update(Notification notification)
        {
            EntityEntry<Notification> updatedEntry = _healthCareContext.Notifications.Attach(notification);
            _healthCareContext.Entry(notification).State = EntityState.Modified;
            return updatedEntry.Entity;
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }

}

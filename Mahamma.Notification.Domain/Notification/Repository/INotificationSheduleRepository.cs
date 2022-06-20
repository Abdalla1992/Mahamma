using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.Notification.Dto;
using Mahamma.Notification.Domain.Notification.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Repository
{
    public interface INotificationSheduleRepository : IRepository<Entity.NotificationShedule>
    {
        void CreateNotificationSchedule(NotificationShedule notificationShedule);
        //void CheckUserHaveSetting(long userId);

        public Task<NotificationShedule> GetnotificationSheduleByUserId(long userId);
        Task<NotificationShedule> GetEntityById(int id);
        void UpdateNotificationShedule(NotificationShedule notiicationSchedule);
        Task<NotificationScheduleDto>  GetUsernotificationShedule(long id);
    }
}

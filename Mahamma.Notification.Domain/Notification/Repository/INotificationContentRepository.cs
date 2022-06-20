using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.Notification.Dto;
using Mahamma.Notification.Domain.Notification.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Repository
{
    //[TODO:refactor]: notifications content is not aggregate root yet it have a repository
    public interface INotificationContentRepository : IRepository<NotificationContent>
    {
        void AddNotificationContent(Entity.NotificationContent notificationContent);
        Task<NotificationContent> GetNotificationContentByNotificationId(int notificationId , int languageId);
        Task<PageList<NotificationContent>> GetNotificationList(int skipCount, int takeCount, Expression<Func<NotificationContent, bool>> filter = null, string includeProperties = "");
        Task<PageList<NotificationContentDto>> GetMappedNotificationList(int skipCount, int takeCount, Expression<Func<NotificationContent, bool>> filter = null, string includeProperties = "");
    }
}

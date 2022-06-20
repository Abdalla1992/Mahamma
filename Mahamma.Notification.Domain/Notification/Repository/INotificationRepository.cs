using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Notification.Domain._SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Repository
{
    public interface INotificationRepository : IRepository<Entity.Notification>
    {

        void AddNotification(Entity.Notification notification);
        Task<PageList<Entity.Notification>> GetNotificationList(int skipCount, int takeCount, Expression<Func<Entity.Notification, bool>> filter = null);
        Task<PageList<Domain.Notification.Entity.Notification>> GetNotificationListForFirebaseUsers(int skipCount, int takeCount, Expression<Func<Domain.Notification.Entity.Notification, bool>> filter = null);
        Task<PageList<Domain.Notification.Entity.Notification>> GetReadyToSendNotificationList(int skipCount, int takeCount, Expression<Func<Domain.Notification.Entity.Notification, bool>> filter = null);
        void UpdateNotification(Entity.Notification notification);
        Task<int> GetNotificationCount(Expression<Func<Entity.Notification, bool>> filter = null);
    }
}

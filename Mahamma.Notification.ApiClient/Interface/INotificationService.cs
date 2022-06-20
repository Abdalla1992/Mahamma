using Mahamma.Notification.ApiClient.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.ApiClient.Interface
{
    public interface INotificationService
    {
        Task<bool> CreateNotification(NotificationDto notificationDto);
        Task<bool> CreateNotificationList(List<NotificationDto> notificationListDto);
    }
}

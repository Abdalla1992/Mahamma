using Mahamma.Notification.Domain.Notification.Dto;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Dto;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.Helper.SendFirebaseNotification
{
    public interface ISendFirebaseNotification
    {
        Task<FirebaseNotificationResponse> SendNotification(PushNotificationModel notificationModel);
    }
}

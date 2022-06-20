using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Helper.EmailSending.IService;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.AppService.Notification.Helper.SendFirebaseNotification;
using Mahamma.Notification.AppService.Settings;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Enum;
using Mahamma.Notification.Domain.Notification.Repository;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.SendDesktopNotification
{
    public class SendDesktopNotificationCommandHandler : IRequestHandler<SendDesktopNotificationCommand, int>
    {
        #region Prop
        private readonly INotificationRepository _notificationRepository;
        private readonly IFirebaseNotificationTokensRepository _firebaseNotificationTokensRepository;
        private readonly ISendFirebaseNotification _sendFirebaseNotification;
        private readonly BackGroundServiceSettings _backGroundServiceSettings;
        private readonly IAccountService _accountService;
        private readonly INotificationContentRepository _notificationContentRepository;
        #endregion

        #region Ctor
        public SendDesktopNotificationCommandHandler(INotificationRepository notificationRepository, BackGroundServiceSettings backGroundServiceSettings,
            IAccountService accountService, INotificationContentRepository notificationContentRepository, IFirebaseNotificationTokensRepository firebaseNotificationTokensRepository, ISendFirebaseNotification sendFirebaseNotification)//, IWorkerServiceParallelHelper workerServiceParallelHelper ,)
        {
            _notificationRepository = notificationRepository;
            _backGroundServiceSettings = backGroundServiceSettings;
            _accountService = accountService;
            _notificationContentRepository = notificationContentRepository;
            _firebaseNotificationTokensRepository = firebaseNotificationTokensRepository;
            _sendFirebaseNotification = sendFirebaseNotification;
        }
        #endregion


        public async Task<int> Handle(SendDesktopNotificationCommand request, CancellationToken cancellationToken)
        {
            bool isSent = false;
            PageList<Domain.Notification.Entity.Notification> notificationList = await _notificationRepository.GetNotificationListForFirebaseUsers(request.SkipCount, _backGroundServiceSettings.TakeCount,
                n => !n.IsRead && n.NotificationSendingStatusId == NotificationSendingStatus.New.Id 
                && n.NotificationSendingTypeId == NotificationSendingType.DeviceNotification.Id);

            if (notificationList != null && notificationList.DataList.Any())
            {
                foreach (var item in notificationList.DataList)
                {
                    UserDto userDto = _accountService.GetUserByIdForBackgroundService(item.ReceiverUserId);
                    var firebaseNotificationToken = await _firebaseNotificationTokensRepository.GetFirebaseNotificationToken(userDto.Id);
                    NotificationContent content = await _notificationContentRepository.GetNotificationContentByNotificationId(item.Id, userDto.LanguageId);
                    if(firebaseNotificationToken != null)
                    {
                        Domain.UserPushNotificationTokens.Dto.FirebaseNotificationResponse response = await _sendFirebaseNotification.SendNotification(new()
                        { 
                            Token = firebaseNotificationToken.FirebaseToken,
                            Body = content.Body,
                            Title = content.Title,
                        });
                        isSent = response.IsSuccess;
                    }
                    if (isSent)
                    {
                        item.UpdateNotificationSendingStatus(NotificationSendingStatus.Sent.Id);
                        _notificationRepository.UpdateNotification(item);
                    }
                }
                await _notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            return notificationList.TotalCount;
        }
    }
}

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

namespace Mahamma.Notification.AppService.Notification.SendNotificationByEmail
{
    public class SendNotificationByEmailCommandHandler : IRequestHandler<SendNotificationByEmailCommand, int>
    {
        #region Prop
        private readonly INotificationRepository _notificationRepository;
        private readonly ISendFirebaseNotification _sendFirebaseNotification;
        private readonly BackGroundServiceSettings _backGroundServiceSettings;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IAccountService _accountService;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly INotificationContentRepository _notificationContentRepository;
        #endregion

        #region Ctor
        public SendNotificationByEmailCommandHandler(INotificationRepository notificationRepository, BackGroundServiceSettings backGroundServiceSettings,
            IEmailSenderService emailSenderService, IAccountService accountService, INotificationResourceReader notificationResourceReader,
            INotificationContentRepository notificationContentRepository, ISendFirebaseNotification sendFirebaseNotification)
        {
            _notificationRepository = notificationRepository;
            _backGroundServiceSettings = backGroundServiceSettings;
            _emailSenderService = emailSenderService;
            _accountService = accountService;
            _notificationResourceReader = notificationResourceReader;
            _notificationContentRepository = notificationContentRepository;
            _sendFirebaseNotification = sendFirebaseNotification;
        }
        #endregion

        public async Task<int> Handle(SendNotificationByEmailCommand request, CancellationToken cancellationToken)
        {
            PageList<Domain.Notification.Entity.Notification> notificationList = await _notificationRepository.GetReadyToSendNotificationList(request.SkipCount, _backGroundServiceSettings.TakeCount,
                n => !n.IsRead && n.NotificationSendingStatusId == NotificationSendingStatus.New.Id
                && n.NotificationSendingTypeId == NotificationSendingType.Email.Id);

            if (notificationList != null && notificationList.DataList.Any())
            {
                foreach (var item in notificationList.DataList)
                {
                    UserDto userDto = _accountService.GetUserByIdForBackgroundService(item.ReceiverUserId);
                    NotificationContent content = await _notificationContentRepository.GetNotificationContentByNotificationId(item.Id, userDto.LanguageId);
                    bool isSent = await _emailSenderService.SendEmail(content.Title, content.Body, userDto.Email);
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

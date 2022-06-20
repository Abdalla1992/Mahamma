using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.Domain.Language.Enum;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Enum;
using Mahamma.Notification.Domain.Notification.Repository;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.AddNotificationList
{
    public class AddNotificationListCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public List<Domain.Notification.Dto.NotificationDto> NotificationList { get; set; }
    }
    class AddNotificationListCommandHandler : IRequestHandler<AddNotificationListCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationContentRepository _notificationContentRepository;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;


        #endregion

        #region Ctor
        public AddNotificationListCommandHandler(INotificationContentRepository notificationContentRepository,
            INotificationResourceReader notificationResourceReader, IAccountService accountService, INotificationRepository notificationRepository)
        {
            _notificationContentRepository = notificationContentRepository;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
            _notificationRepository = notificationRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddNotificationListCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            foreach (var item in request.NotificationList)
            {
                if (item.NotificationSendingTypeId == NotificationSendingType.All.Id)
                {
                    #region Create Email Notification
                    Domain.Notification.Entity.Notification emailNotification = new();
                    emailNotification.CreateNotification(item.WorkSpaceId, item.ProjectId, item.TaskId, item.MeetingId, NotificationSendingType.Email.Id, item.NotificationSendingStatusId,
                    item.NotificationTypeId, item.SenderUserId, item.ReceiverUserId, item.IsRead, GetNotificationContents(item));
                    _notificationRepository.AddNotification(emailNotification);
                    #endregion
                    #region Create Push Notification
                    Domain.Notification.Entity.Notification pushNotification = new();
                    pushNotification.CreateNotification(item.WorkSpaceId, item.ProjectId, item.TaskId, item.MeetingId, NotificationSendingType.PushNotification.Id, item.NotificationSendingStatusId,
                    item.NotificationTypeId, item.SenderUserId, item.ReceiverUserId, item.IsRead, GetNotificationContents(item));
                    _notificationRepository.AddNotification(pushNotification);
                    #endregion
                    #region Create Device Notification
                    Domain.Notification.Entity.Notification deviceNotification = new();
                    deviceNotification.CreateNotification(item.WorkSpaceId, item.ProjectId, item.TaskId, item.MeetingId, NotificationSendingType.DeviceNotification.Id, item.NotificationSendingStatusId,
                    item.NotificationTypeId, item.SenderUserId, item.ReceiverUserId, item.IsRead, GetNotificationContents(item));
                    _notificationRepository.AddNotification(deviceNotification);
                    #endregion
                }
                else
                {
                    Domain.Notification.Entity.Notification notification = new();
                    notification.CreateNotification(item.WorkSpaceId, item.ProjectId, item.TaskId, item.MeetingId, item.NotificationSendingTypeId, item.NotificationSendingStatusId,
                    item.NotificationTypeId, item.SenderUserId, item.ReceiverUserId, item.IsRead, GetNotificationContents(item));
                    _notificationRepository.AddNotification(notification);
                }
            }
            if (await _notificationContentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = "Data Added Successfully";
            }
            else
            {
                response.Result.CommandMessage = "Failed to add the new Notification. Try again shortly.";
            }
            return response;
        }

        private static List<NotificationContent> GetNotificationContents(Domain.Notification.Dto.NotificationDto item)
        {
            List<NotificationContent> result = new();

            NotificationContent notificationContentEnglish = new();
            NotificationContent notificationContentArabic = new();

            notificationContentEnglish.CreateNotificationContent(item.NotificationTitleEnglish, item.NotificationBodyEnglish, Language.English.Id, item.Id);
            notificationContentArabic.CreateNotificationContent(item.NotificationTitleArabic, item.NotificationBodyArabic, Language.Arabic.Id, item.Id);
            result = new() { notificationContentEnglish, notificationContentArabic };
            return result;
        }
    }
}

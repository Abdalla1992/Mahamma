using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Enum;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Api.Application.DomainEventHandler.Meeting
{
    public class MeetingUpdatedEventHandler : INotificationHandler<MeetingUpdatedEvent>
    {
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        private readonly INotificationResourceReader _notificationResourceReader;

        public MeetingUpdatedEventHandler(IMediator mediator, INotificationResourceReader notificationResourceReader, INotificationService notificationService)
        {
            _mediator = mediator;
            _notificationResourceReader = notificationResourceReader;
            _notificationService = notificationService;
        }

        public async Task Handle(MeetingUpdatedEvent request, CancellationToken cancellationToken)
        {
            #region Send Notification
            if (request.Meeting.Members?.Count > 0)
            {
                List<NotificationDto> notificationListDto = new();
                foreach (var member in request.Meeting.Members)
                {
                    notificationListDto.Add(PrepareNotification(request.Meeting.Title, request.Meeting.Id, request.CreatorUser, member.UserId));
                }
                await _notificationService.CreateNotificationList(notificationListDto);
            }
            #endregion
        }

        private NotificationDto PrepareNotification(string meetingTitle ,int meetingId, UserDto sender, long receiverUserId)
        {
            return new NotificationDto
            {
                SenderUserId = sender.Id,
                ReceiverUserId = receiverUserId,
                MeetingId = meetingId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = NotificationType.MeetingUpdated.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.MeetingUpdated.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.MeetingUpdated.Id, Language.English.Id) + "{1}", sender.FullName, meetingTitle),
                NotificationBodyArabic = string.Format("{0}" + GetNotificationBody(NotificationType.MeetingUpdated.Id, Language.Arabic.Id) + "{1}", sender.FullName, meetingTitle),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.MeetingUpdated.Id, Language.Arabic.Id)
            };
        }
        private string GetNotificationBody(int NotificationTypeId, int languageId)
        {
            var name = NotificationType.From(NotificationTypeId);
            return _notificationResourceReader.GetKeyValue(name + "Body", languageId);
        }
        private string GetNotificationTitle(int NotificationTypeId, int languageId)
        {
            var name = NotificationType.From(NotificationTypeId);
            return _notificationResourceReader.GetKeyValue(name + "Title", languageId);
        }
    }
}

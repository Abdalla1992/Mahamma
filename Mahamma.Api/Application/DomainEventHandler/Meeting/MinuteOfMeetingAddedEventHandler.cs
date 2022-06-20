using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Enum;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Enum;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Api.Application.DomainEventHandler.Meeting
{
    public class MinuteOfMeetingAddedEventHandler : INotificationHandler<MinuteOfMeetingAddedEvent>
    {
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        private readonly INotificationResourceReader _notificationResourceReader;

        public MinuteOfMeetingAddedEventHandler(IMediator mediator, INotificationResourceReader notificationResourceReader, INotificationService notificationService)
        {
            _mediator = mediator;
            _notificationResourceReader = notificationResourceReader;
            _notificationService = notificationService;
        }

        public async Task Handle(MinuteOfMeetingAddedEvent request, CancellationToken cancellationToken)
        {
            #region Send Notification
            if (request.Members?.Count > 0)
            {
                try
                {
                    List<NotificationDto> notificationListDto = new();
                    foreach (var member in request.Members.Where(m => m.UserId != request.CreatorUser.Id))
                    {
                        notificationListDto.Add(PrepareNotification(request.MeetingTitle, request.MeetingId, request.MinutesOfMeeting, request.CreatorUser, member.UserId));
                    }
                    await _notificationService.CreateNotificationList(notificationListDto);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
        }

        private NotificationDto PrepareNotification(string meetingTitle ,int meetingId, List<Domain.Meeting.Dto.MinuteOfMeetingActionDto> minutesOfMeeting, UserDto sender, long receiverUserId)
        {
            return new NotificationDto
            {
                SenderUserId = sender.Id,
                ReceiverUserId = receiverUserId,
                MeetingId = meetingId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = NotificationType.MinuteOfMeetingAdded.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.MinuteOfMeetingAdded.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0} {1}" + GetNotificationBody(NotificationType.MinuteOfMeetingAdded.Id, Language.English.Id, minutesOfMeeting) , sender.FullName, meetingTitle),
                NotificationBodyArabic = string.Format("{0} {1}" + GetNotificationBody(NotificationType.MinuteOfMeetingAdded.Id, Language.Arabic.Id, minutesOfMeeting), sender.FullName, meetingTitle),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.MinuteOfMeetingAdded.Id, Language.Arabic.Id)
            };
        }
        private string GetNotificationBody(int NotificationTypeId, int languageId, List<Domain.Meeting.Dto.MinuteOfMeetingActionDto> minutesOfMeeting)
        {
            var name = NotificationType.From(NotificationTypeId);
            var translatedPart =  _notificationResourceReader.GetKeyValue(name + "Body", languageId);
            return $"{translatedPart} </br> <table border='1' >  <tr>Title</tr> <tr>Type</tr> {String.Join(' ', minutesOfMeeting.Select(mom => $"<td> {mom.ActionTitle} </td> <td>{_notificationResourceReader.GetKeyValue(Base.Dto.Enum.Enumeration.FromValue<MinuteOfMeetingLevel>(mom.ActionLevel).Name, languageId) }</td>"))} </table>";
        }
        private string GetNotificationTitle(int NotificationTypeId, int languageId)
        {
            var name = NotificationType.From(NotificationTypeId);
            return _notificationResourceReader.GetKeyValue(name + "Title", languageId);
        }
    }
}

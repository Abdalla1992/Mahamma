using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Task.Event;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Enum;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Api.Application.DomainEventHandler.Tasks
{
    public class TaskInProgressEventHandler : INotificationHandler<TaskInProgressEvent>
    {
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        private readonly INotificationResourceReader _notificationResourceReader;

        public TaskInProgressEventHandler(IMediator mediator, INotificationResourceReader notificationResourceReader, INotificationService notificationService)
        {
            _mediator = mediator;
            _notificationResourceReader = notificationResourceReader;
            _notificationService = notificationService;
        }

        public async Task Handle(TaskInProgressEvent taskEvent, CancellationToken cancellationToken)
        {
            #region Send Notification
            if (taskEvent.Task.TaskMembers?.Count > 0)
            {
                List<NotificationDto> notificationListDto = new();
                foreach (var member in taskEvent.Task.TaskMembers)
                {
                    notificationListDto.Add(PrepareNotification(taskEvent.Task.Name, taskEvent.Task.Id, taskEvent.Task.ParentTaskId != null, member.UserId));
                }
                await _notificationService.CreateNotificationList(notificationListDto);
            }
            #endregion  
        }

        private NotificationDto PrepareNotification(string taskTitle ,int taskId, bool isSubTask, long receiverUserId)
        {
            NotificationType notificationType = !isSubTask ? NotificationType.TaskIsInProgress : NotificationType.SubTaskIsInProgress;
            var ss = new NotificationDto()
                {
                    SenderUserId = 0,
                    ReceiverUserId = receiverUserId,
                    TaskId = taskId,
                    NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                    NotificationSendingTypeId = NotificationSendingType.All.Id,
                    NotificationTypeId = notificationType.Id,
                    IsRead = false,
                    NotificationTitleEnglish = GetNotificationTitle(notificationType.Id, Language.English.Id),
                    NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(notificationType.Id, Language.English.Id) + "{1}", "", taskTitle),
                    NotificationBodyArabic = string.Format("{0}" + GetNotificationBody(notificationType.Id, Language.Arabic.Id) + "{1}", "", taskTitle),
                    NotificationTitleArabic = GetNotificationTitle(notificationType.Id, Language.Arabic.Id)
                };
            return ss;
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

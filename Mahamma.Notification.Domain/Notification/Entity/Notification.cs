using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Entity
{
    public class Notification : Entity<int>, IAggregateRoot
    {
        public int? WorkSpaceId { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public int? MeetingId { get; set; }
        public int NotificationSendingTypeId { get; set; }
        public int NotificationSendingStatusId { get; set; }
        public int NotificationTypeId { get; set; }
        public long SenderUserId { get; set; }
        public long ReceiverUserId { get; set; }
        public bool IsRead { get; set; }
        public List<NotificationContent> NotificationContents { get; set; }

        public void CreateNotification(int? workspaceId,int? projectId,int? taskId, int? meetingId , int notificationSendingTypeId,
            int notificationSendingStatusId,int notificationTypeId,long senderUserId,long receiverUserId,bool isRead, List<NotificationContent> notificationContents)
        {
            WorkSpaceId = workspaceId;
            ProjectId = projectId;
            TaskId = taskId;
            MeetingId = meetingId;
            NotificationSendingTypeId = notificationSendingTypeId;
            NotificationSendingStatusId = notificationSendingStatusId;
            NotificationTypeId = notificationTypeId;
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            IsRead = isRead;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            NotificationContents = notificationContents;
        }

        public void UpdateNotificationSendingStatus(int notificationSendingStatusId)
        {
            NotificationSendingStatusId = notificationSendingStatusId;
        }

        public void DeleteNotification()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }



    }
}

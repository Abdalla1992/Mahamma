using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.ApiClient.Dto.Notification
{
    public class NotificationDto
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
        public string NotificationTitleEnglish { get; set; }
        public string NotificationBodyEnglish { get; set; }
        public string NotificationTitleArabic { get; set; }
        public string NotificationBodyArabic { get; set; }
    }
}

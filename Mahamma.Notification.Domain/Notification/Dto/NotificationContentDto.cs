using Mahamma.Base.Dto.Dto;

namespace Mahamma.Notification.Domain.Notification.Dto
{
    public class NotificationContentDto : BaseDto<int>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int LanguageId { get; set; }
        public int NotificationId { get; set; }
        public NotificationDto Notification { get; set; }
        public string CreationDuration { get; set; }
    }
}

using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Entity
{
    public class NotificationContent : Entity<int>, IAggregateRoot
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int LanguageId { get; set; }
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        public void CreateNotificationContent(string title,string body,int languageId,int notificationId)
        {
            Title = title;
            Body = body;
            LanguageId = languageId;
            NotificationId = notificationId;

            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
    }
}

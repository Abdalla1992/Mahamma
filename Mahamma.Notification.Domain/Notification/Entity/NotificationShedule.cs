using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Entity
{
    public class NotificationShedule : Entity<int>, IAggregateRoot
    {
        #region Prop
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long UserId { get; set; }
        public int NotificationScheduleTypeId { get; set; }
        public int? WeekDayId { get; set; }
        public int? MonthDayId { get; set; }
        #endregion


       

        #region Methods
        public void CreateNotificationShedule(string fromParse, string toParse, long userId, int notificationScheduleTypeId, int? weekDayId, int? monthDayId)
        {
            From = DateTime.TryParse(fromParse, out DateTime res) ? res : DateTime.Now;
            To = DateTime.TryParse(toParse, out DateTime ress) ? ress : DateTime.Now;
            UserId = userId;
            NotificationScheduleTypeId = notificationScheduleTypeId;
            WeekDayId = weekDayId;
            MonthDayId = monthDayId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void UpdateNotificationShedule(string from, string to, long userId, int notificationScheduleTypeId, int? weekDayId, int? monthDayId)
        { 
            From = DateTime.TryParse(from, out DateTime res) ? res : DateTime.Now;
            From = DateTime.TryParse(to, out DateTime ress) ? ress : DateTime.Now;
            UserId = userId;
            NotificationScheduleTypeId = notificationScheduleTypeId;
            WeekDayId = weekDayId;
            MonthDayId = monthDayId;
        }
        public void DeleteNotificationShedule()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

        #endregion
    }
}

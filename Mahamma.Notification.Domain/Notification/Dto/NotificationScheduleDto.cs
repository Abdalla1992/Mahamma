using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Dto
{
    public class NotificationScheduleDto : BaseDto<int>
    {
        #region Prop
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public long UserId { get; set; }
        public int NotificationScheduleTypeId { get; set; }
        public int? WeekDayId { get; set; }
        public int? MonthDayId { get; set; }
        #endregion
    }
}

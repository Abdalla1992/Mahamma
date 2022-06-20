using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.UpdateNotiicationSchedule
{
    public class UpdateNotiicationScheduleCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public int NotificationScheduleTypeId { get; set; }
        [DataMember]
        public int? WeekDayId { get; set; }
        [DataMember]
        public int? MonthDayId { get; set; }
        #endregion
    }

}

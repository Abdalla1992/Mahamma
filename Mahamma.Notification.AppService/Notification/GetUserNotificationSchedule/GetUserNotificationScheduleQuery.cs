using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Notification.Domain.Notification.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.GetUserNotificationSchedule
{
    public class GetUserNotificationScheduleQuery : IRequest<ValidateableResponse<ApiResponse<NotificationScheduleDto>>>
    {
        #region Props
        [DataMember]
        public long UserId { get; set; }
        #endregion

        #region CTRS
        public GetUserNotificationScheduleQuery(long id)
        {
            UserId = id;
        }
        #endregion
    }
}


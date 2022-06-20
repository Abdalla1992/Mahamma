using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.AddNotification
{
   public class AddNotificationCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Pقop
        [DataMember]
        public int NotificationSendingTypeId { get; set; }
        [DataMember]
        public int NotificationSendingStatusId { get; set; }
        [DataMember]
        public int NotificationTypeId { get; set; }
        [DataMember]
        public long SenderUserId { get; set; }
        [DataMember]
        public long ReceiverUserId { get; set; }
        [DataMember]
        public bool IsRead { get; set; }
        #endregion

    }
}

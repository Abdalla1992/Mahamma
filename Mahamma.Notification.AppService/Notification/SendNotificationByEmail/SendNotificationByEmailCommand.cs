using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.SendNotificationByEmail
{
    public class SendNotificationByEmailCommand : IRequest<int>
    {
        [DataMember]
        public int SkipCount { get; set; }

        public SendNotificationByEmailCommand(int skipCount)
        {
            SkipCount = skipCount;
        }



    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.SendDesktopNotification
{
    public class SendDesktopNotificationCommand : IRequest<int>
    {
        [DataMember]
        public int SkipCount { get; set; }

        public SendDesktopNotificationCommand(int skipCount)
        {
            SkipCount = skipCount;
        }



    }
}

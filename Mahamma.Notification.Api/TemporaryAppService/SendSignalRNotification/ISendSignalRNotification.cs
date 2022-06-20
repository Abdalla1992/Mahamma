using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.TemporaryAppService
{
    public interface ISendSignalRNotification
    {
        Task<bool> Handle(int skipCount, CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.Hub
{
    public interface IHubClient
    {
        Task InformClient(int notificationCount);
    }
}

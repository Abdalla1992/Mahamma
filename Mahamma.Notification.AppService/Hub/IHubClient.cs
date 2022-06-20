using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Hub
{
    public interface IHubClient
    {
        Task InformClient(string message);
    }
}

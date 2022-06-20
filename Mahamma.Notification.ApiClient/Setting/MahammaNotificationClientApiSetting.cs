using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.ApiClient.Setting
{
    public class MahammaNotificationClientApiSetting
    {
        public string NotificationUrl { get; set; }
        public string CreateNotificationUrl { get; set; } = "api/Notification/Add";
        public string CreateNotificationListUrl { get; set; } = "api/Notification/AddList";
      
    }
}

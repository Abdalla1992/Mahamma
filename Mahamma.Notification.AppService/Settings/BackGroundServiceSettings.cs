using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Settings
{
    public class BackGroundServiceSettings
    {
        public int TakeCount { get; set; }
        public int WorkEveryInMilliseconds { get; set; }
        public bool UseParallel { get; set; }
        public int ThreadCount { get; set; }
    }
}

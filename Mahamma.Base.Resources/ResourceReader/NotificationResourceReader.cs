using Mahamma.Base.Resources.Enum;
using Mahamma.Base.Resources.IResourceReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mahamma.Base.Resources.ResourceReader
{
    internal class NotificationResourceReader : Base.BaseFileReader, INotificationResourceReader
    {
        #region CTRS
        public NotificationResourceReader():base(LocalizationType.Notification)
        { }
        #endregion
    }
}

using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Enum
{
    public class NotificationSendingType : Enumeration
    {
        #region Enum Value
        public static NotificationSendingType Email = new(1, nameof(Email));
        public static NotificationSendingType PushNotification = new(2, nameof(PushNotification));
        public static NotificationSendingType DeviceNotification = new(3, nameof(DeviceNotification));
        public static NotificationSendingType All = new(4, nameof(All));
        #endregion


        #region Ctor
        public NotificationSendingType(int id, string name) : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<NotificationSendingType> List() => new[] { Email, PushNotification, All };

        public static NotificationSendingType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationSendingType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static NotificationSendingType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationSendingType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        #endregion

    }
}

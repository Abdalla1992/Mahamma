using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Enum
{
    public class NotificationSendingStatus : Enumeration
    {
        #region Enum Value
        public static NotificationSendingStatus New = new(1, nameof(New));
        public static NotificationSendingStatus Sent = new(2, nameof(Sent));
        public static NotificationSendingStatus Faild = new(3, nameof(Faild));
        #endregion


        #region Ctor
        public NotificationSendingStatus(int id, string name) : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<NotificationSendingStatus> List() => new[] { New, Sent, Faild };

        public static NotificationSendingStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationSendingStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static NotificationSendingStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationSendingStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        #endregion

    }
}

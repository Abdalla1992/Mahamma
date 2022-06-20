using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Enum
{
    public class NotificationWeekDay : Enumeration
    {
        #region Prop
        public static NotificationWeekDay Saturday = new(1, nameof(Saturday));
        public static NotificationWeekDay Sunday = new(2, nameof(Sunday));
        public static NotificationWeekDay Monday = new(3, nameof(Monday));
        public static NotificationWeekDay Tuesday = new(4, nameof(Tuesday));
        public static NotificationWeekDay Wednesday = new(5, nameof(Wednesday));
        public static NotificationWeekDay Thursday = new(6, nameof(Thursday));
        public static NotificationWeekDay Friday = new(7, nameof(Friday));
        #endregion

        #region Ctor
        public NotificationWeekDay(int id, string name) : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<NotificationWeekDay> List() => new[] { Saturday, Sunday, Monday,Tuesday,Wednesday,Thursday,Friday };

        public static NotificationWeekDay FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationSendingStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static NotificationWeekDay From(int id)
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

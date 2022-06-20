using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Enum
{
    public class NotificationSheduleType : Enumeration
    {
        #region Prop
        public static NotificationSheduleType Daily = new(1, nameof(Daily));
        public static NotificationSheduleType Weekly = new(2, nameof(Weekly));
        public static NotificationSheduleType Monthly = new(3, nameof(Monthly));
        #endregion

        #region Ctor
        public NotificationSheduleType(int id, string name) : base(id, name)
        {

        }
        #endregion

        #region Methods
        public static IEnumerable<NotificationSheduleType> List() => new[] { Daily, Weekly, Monthly };

        public static NotificationSheduleType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationSendingStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static NotificationSheduleType From(int id)
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

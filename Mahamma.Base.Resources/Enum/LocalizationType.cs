using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Resources.Enum
{
   public  class LocalizationType : Enumeration
    {

        #region Enum Value
        public static LocalizationType Message = new(1, nameof(Message));
        public static LocalizationType Language = new(2, nameof(Language));
        public static LocalizationType Notification = new(3, nameof(Notification));
        #endregion

        #region Ctor
        public LocalizationType(int id, string name) : base(id, name)
        {
        }
        #endregion


        #region Methods
        public static IEnumerable<LocalizationType> List() => new[] {Message,Language,Notification };

        public static LocalizationType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationSendingStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static LocalizationType From(int id)
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

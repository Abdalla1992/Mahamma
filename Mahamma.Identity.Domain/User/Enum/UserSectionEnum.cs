using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.User.Enum
{
    public class UserSectionEnum : Enumeration
    {
        #region Enum Values
        public static UserSectionEnum Task = new(1, nameof(Task));
        public static UserSectionEnum Bio = new(1, nameof(Bio));
        public static UserSectionEnum WorkHistory = new(1, nameof(WorkHistory));
        public static UserSectionEnum RejectedTask = new(1, nameof(RejectedTask));
       
        #endregion
        #region CTRS
        public UserSectionEnum(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<UserSectionEnum> List() => new[] { Task, Bio, WorkHistory, RejectedTask };
        public static UserSectionEnum FromName(string name)
        {
            var value = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (value == null)
            {
                throw new Exception($"Possible values for UserSectionEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return value;
        }

        public static UserSectionEnum From(int id)
        {
            var value = List().SingleOrDefault(s => s.Id == id);

            if (value == null)
            {
                throw new Exception($"Possible values for UserProfileStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return value;
        }
        #endregion
    }
}

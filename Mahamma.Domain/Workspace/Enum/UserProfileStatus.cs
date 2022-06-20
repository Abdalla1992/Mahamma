using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Workspace.Enum
{
    public class UserProfileStatus : Enumeration
    {
        #region Enum Values
        public static UserProfileStatus Registered = new(1, nameof(Registered));
        public static UserProfileStatus ProfileCompleted = new(2, nameof(ProfileCompleted));
        public static UserProfileStatus CompanyCreated = new(3, nameof(CompanyCreated));
        public static UserProfileStatus FirstWorkspaceCreated = new(4, nameof(FirstWorkspaceCreated));
        #endregion
        #region CTRS
        public UserProfileStatus(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<UserProfileStatus> List() => new[] { Registered, ProfileCompleted, CompanyCreated, FirstWorkspaceCreated };
        public static UserProfileStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for UserProfileStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static UserProfileStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for UserProfileStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion
    }
}

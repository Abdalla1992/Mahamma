using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Company.Enum
{
    public class InvitationStatus : Enumeration
    {
        #region Enum Values
        public static InvitationStatus New = new(1, nameof(New));
        public static InvitationStatus Sent = new(2, nameof(Sent));
        public static InvitationStatus Opened = new(3, nameof(Opened));
        #endregion
        #region CTRS
        public InvitationStatus(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<InvitationStatus> List() => new[] { New, Sent, Opened };
        public static InvitationStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for InvitationStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static InvitationStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for InvitationStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion
    }
}

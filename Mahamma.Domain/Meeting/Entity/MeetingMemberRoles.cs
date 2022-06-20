using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Meeting.Entity
{
    public class MeetingMemberRoles : Entity<int>
    {
        #region Prop
        public long UserId { get; private set; }
        public int MeetingRoleId { get; private set; }
        #endregion

        #region Navigation
        #endregion

        public void CreateMemberRole(long userId, int roleId)
        {
            UserId = userId;
            MeetingRoleId = roleId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void DeleteMemberRole()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
    }
}

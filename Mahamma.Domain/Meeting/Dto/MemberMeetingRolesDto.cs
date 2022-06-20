using Mahamma.Base.Domain;
using Mahamma.Domain.Meeting.Enum;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Dto
{
    public class MemberMeetingRolesDto : Entity<int>
    {
        #region Prop
        public long UserId { get; set; }
        public List<int> MeetingRoles { get; set; }
        #endregion
    }
}

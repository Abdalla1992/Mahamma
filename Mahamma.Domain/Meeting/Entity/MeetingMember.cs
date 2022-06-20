using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Entity
{
    public class MeetingMember : Entity<int>
    {
        #region Prop
        public long UserId { get; private set; }
        public int MeetingId { get; private set; }
        public bool? CanMakeMinuteOfMeeting { get; private set; }
        public bool? InvitationAccepted { get; private set; }
        public bool? Attended { get; private set; }
        #endregion

        #region Navigation
        public List<MeetingMemberRoles> MeetingRoles { get; private set; } = new();
        #endregion

        public void CreateMeetingMember(long userId, int meetingId, List<int> meetingRoles, bool? canMakeMinuteOfMeeting = false, bool? invitationAccepted = false)
        {
            UserId = userId;
            MeetingId = meetingId;
            CanMakeMinuteOfMeeting = canMakeMinuteOfMeeting;
            InvitationAccepted = invitationAccepted;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;

            MeetingRoles?.ForEach(memberRole => memberRole.DeleteMemberRole());
            meetingRoles?.ForEach(roleId =>
            {
                MeetingMemberRoles meetingMemberRole = new MeetingMemberRoles();
                meetingMemberRole.CreateMemberRole(userId, roleId);
                MeetingRoles.Add(meetingMemberRole);
            });
        }

        public void DeleteMeetingMember()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

        internal void HaveAttended()
        {
            Attended = true;
        }
    }
}

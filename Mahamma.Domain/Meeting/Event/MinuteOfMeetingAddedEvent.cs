using Mahamma.Base.Domain.Event;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Event
{
    public class MinuteOfMeetingAddedEvent : INotificationRequest<bool>
    {
        public int MeetingId { get; set; }
        public string MeetingTitle { get; set; }
        public List<MeetingMember> Members { get; set; }
        public List<Dto.MinuteOfMeetingActionDto> MinutesOfMeeting { get; set; }
        public UserDto CreatorUser { get; set; }
        public MinuteOfMeetingAddedEvent(int meetingId, string meetingTitle, List<MeetingMember> members, List<Dto.MinuteOfMeetingActionDto> minutesOfMeeting, UserDto creatorUser)
        {
            MeetingId = meetingId;
            MeetingTitle = meetingTitle;
            Members = members;
            MinutesOfMeeting = minutesOfMeeting;
            CreatorUser = creatorUser;
        }
    }
}

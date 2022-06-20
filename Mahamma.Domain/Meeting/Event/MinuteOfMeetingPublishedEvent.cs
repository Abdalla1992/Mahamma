using Mahamma.Base.Domain.Event;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Event
{
    public class MinuteOfMeetingPublishedEvent : INotificationRequest<bool>
    {
        public Entity.Meeting Meeting { get; set; }
        public UserDto CreatorUser { get; set; }
        public MinuteOfMeetingPublishedEvent(Entity.Meeting meeting, UserDto creatorUser)
        {
            Meeting = meeting;
            CreatorUser = creatorUser;
        }
    }
}

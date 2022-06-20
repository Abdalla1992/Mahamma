using Mahamma.Base.Domain.Event;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Event
{
    public class MeetingAddedEvent : INotificationRequest<bool>
    {
        public Entity.Meeting Meeting { get; set; }
        public UserDto CreatorUser { get; set; }
        public MeetingAddedEvent(Entity.Meeting meeting, UserDto creatorUser)
        {
            Meeting = meeting;
            CreatorUser = creatorUser;
        }
    }
}

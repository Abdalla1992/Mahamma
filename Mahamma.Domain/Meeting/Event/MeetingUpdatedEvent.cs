using Mahamma.Base.Domain.Event;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Event
{
    public class MeetingUpdatedEvent : INotificationRequest<bool>
    {
        public Entity.Meeting Meeting { get; set; }
        public UserDto CreatorUser { get; set; }
        public MeetingUpdatedEvent(Entity.Meeting meeting, UserDto creatorUser)
        {
            Meeting = meeting;
            CreatorUser = creatorUser;
        }
    }
}

using Mahamma.Base.Domain.Event;
using Mahamma.Domain.Task.Entity;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Collections.Generic;

namespace Mahamma.Domain.Task.Event
{
    public class TaskDeletedEvent : INotificationRequest<bool>
    {
        public Entity.Task Task { get; set; }
        public UserDto CreatorUser { get; set; }
        public TaskDeletedEvent(Entity.Task meeting, UserDto creatorUser)
        {
            Task = meeting;
            CreatorUser = creatorUser;
        }
    }
}

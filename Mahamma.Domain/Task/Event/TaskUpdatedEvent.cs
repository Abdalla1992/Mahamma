using Mahamma.Base.Domain.Event;
using Mahamma.Domain.Task.Entity;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Collections.Generic;

namespace Mahamma.Domain.Task.Event
{
    public class TaskUpdatedEvent : INotificationRequest<bool>
    {
        public Entity.Task Task { get; set; }
        public UserDto CreatorUser { get; set; }
        public TaskUpdatedEvent(Entity.Task task, UserDto creatorUser)
        {
            Task = task;
            CreatorUser = creatorUser;
        }
    }
}

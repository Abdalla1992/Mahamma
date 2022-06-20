using Mahamma.Base.Domain.Event;
using Mahamma.Domain.Task.Entity;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Collections.Generic;

namespace Mahamma.Domain.Task.Event
{
    public class TaskInProgressEvent : INotificationRequest<bool>
    {
        public Entity.Task Task { get; set; }
        public TaskInProgressEvent(Entity.Task task)
        {
            Task = task;
        }
    }
}

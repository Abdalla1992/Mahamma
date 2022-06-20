using Mahamma.Base.Domain;
using System;

namespace Mahamma.Domain.TaskActivity.Entity
{
    public class TaskActivity : Entity<int> , IAggregateRoot
    {
        #region Props
        public string Action { get; set; }
        public int TaskId { get; set; }
        public int TaskMemberId { get; set; }
        public long? UserId { get; set; }
        #endregion

        #region Methods
        public void LogActivity(string action, int taskId, int taskMemberId)
        {
            Action = action;
            TaskId = taskId;
            TaskMemberId = taskMemberId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void RemoveActivity()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        #endregion
    }
}

using Mahamma.Base.Domain;
using Mahamma.Base.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahamma.Domain.Task.Entity
{
    public class TaskMember : Entity<int>, IAggregateRoot
    {
        #region Props
        public long UserId { get; set; }
        public int TaskId { get; set; }
        public double? Rating { get; set; }
        public int TaskAcceptedRejectedStatus { get; set; }
        #endregion

        #region Nav Props
        public Task Task { get; set; }
        public List<TaskComment> TaskComments { get; set; }
        public List<LikeComment> LikedComments { get; set; }
        #endregion

        #region Methods
        public void CreateTaskMember(long userId, int taskId = default)
        {
            UserId = userId;
            TaskId = taskId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            TaskAcceptedRejectedStatus = Enum.TaskAcceptedRejectedStatus.NotReviewed.Id;
        }
        public void RemoveUserFromTask()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        #endregion

    }
}

using Mahamma.Base.Domain;
using Mahamma.Base.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Task.Entity
{
    public class LikeComment : Entity<int> , IAggregateRoot
    {
        #region Props
        public int? TaskMemberId { get; set; }
        public int? TaskCommentId { get; set; }
        #endregion

        #region Navigation Property
        public TaskMember TaskMember { get; set; }
        public TaskComment TaskComment { get; set; }
        #endregion

        #region Methods
        public void LikeAComment(int taskCommentId, int taskMemberId)
        {
            TaskMemberId = taskMemberId;
            TaskCommentId = taskCommentId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void DisLikeComment()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        #endregion
    }
}

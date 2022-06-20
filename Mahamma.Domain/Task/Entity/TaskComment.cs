using Mahamma.Base.Domain;
using Mahamma.Base.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Task.Entity
{
    public class TaskComment : Entity<int>, IAggregateRoot
    {
        #region Props
        public string Comment { get; set; }
        public int? TaskId { get; set; }
        public int TaskMemberId { get; set; }
        public long? UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public string ImageUrl { get; set; }
        #endregion

        #region Navigation Property
        public List<TaskComment> Replies { get; set; }
        public List<LikeComment> Likes { get; set; }
        #endregion

        #region Methods
        public void CreateComment(string comment, int taskId, int taskMember, int? parentCommentId, string imageUrl, long? userId = null)
        {
            Comment = comment;
            TaskId = taskId;
            TaskMemberId = taskMember;
            ParentCommentId = parentCommentId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            ImageUrl = imageUrl;
            UserId = userId;
        }
        public void DeleteComment()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        #endregion
    }
}

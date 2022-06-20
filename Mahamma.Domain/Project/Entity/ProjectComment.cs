using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Entity
{
    public class ProjectComment : Entity<int>, IAggregateRoot
    {
        #region Props
        public string Comment { get; set; }
        public int? ProjectId { get; set; }
        public int ProjectMemberId { get; set; }
        public long? UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public string ImageUrl { get; set; }
        #endregion

        #region Navigation Property
        public List<ProjectComment> Replies { get; set; }
        public List<ProjectLikeComment> Likes { get; set; }
        #endregion

        #region Methods
        public void CreateComment(string comment,int projectId, int projectMember, int? parentCommentId, string imageUrl, long? userId = null)
        {
            Comment = comment;
            ProjectId = projectId;
            ProjectMemberId = projectMember;
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

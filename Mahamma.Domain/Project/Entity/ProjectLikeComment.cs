using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Entity
{
    public class ProjectLikeComment : Entity<int>, IAggregateRoot
    {
        #region Props
        public int? ProjectMemberId { get; set; }
        public int? ProjectCommentId { get; set; }
        #endregion

        #region Navigation Property
        public ProjectMember ProjectMember { get; set; }
        public ProjectComment ProjectComment { get; set; }
        #endregion

        #region Methods
        public void LikeAComment(int projectCommentId, int projectMemberId)
        {
            ProjectMemberId = projectMemberId;
            ProjectCommentId = projectCommentId;
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

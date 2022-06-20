using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Entity
{
    public class ProjectMember : Entity<int>, IAggregateRoot
    {
        #region Prop
        
        public long UserId { get; set; }
        public int ProjectId { get; set; }
        public double? Rating { get; set; }

        #endregion

        //Can Delete this Property
        #region Navigation
        //public List<ProjectActivity.Entity.ProjectActivity> ProjectActivities { get; set; }
        public List<ProjectComment> ProjectComments { get; set; }
        public List<ProjectLikeComment> ProjectLikeComments { get; set; }
        #endregion

        public void CreateProjectMember(long userId , int projectId )
        {
            UserId = userId;
            ProjectId = projectId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }


        public void DeleteProjectMember()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

    }
}

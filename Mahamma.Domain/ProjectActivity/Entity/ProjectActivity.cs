using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectActivity.Entity
{
   public class ProjectActivity : Entity<int>, IAggregateRoot
    {
        #region Prop    
        public string Action { get; set; }
        public int ProjectMemberId { get; set; }
        public int ProjectId { get; set; }
        #endregion


        #region Methods

        public void LogProjectActivity(string action,int projectId , int projectMemberId)
        {
            Action = action;
            ProjectId = projectId;
            ProjectMemberId = projectMemberId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void DeleteActivity()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
         #endregion
    }
}

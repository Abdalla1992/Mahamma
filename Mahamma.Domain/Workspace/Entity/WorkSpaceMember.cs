using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Workspace.Entity
{
    public class WorkspaceMember : Entity<int>, IAggregateRoot
    {
        #region Prop

        public long UserId { get; set; }
        public int WorkspaceId { get; set; }
        #endregion

        #region Methods
        public void CreateWorkspaceMember(long userId, int workSpaceId)
        {
            UserId = userId;
            WorkspaceId = workSpaceId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void DeleteWorkspaceMember()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

        #endregion
    }
}

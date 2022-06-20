using Mahamma.Base.Domain;
using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Workspace.Entity
{
    public class Workspace : Entity<int>, IAggregateRoot
    {
        #region Props
        public string Name { get; private set; }
        public string ImageUrl { get; private set; }
        public string Color { get; private set; }
        public int CompanyId { get; set; }
        public long CreatorUserId { get; set; }
        #endregion

        #region Navigation Prop
        public List<WorkspaceMember> WorkspaceMembers { get; set; }
        #endregion

        #region Methods
        public void CreateWorkspace(string name, string imageUrl, string color, int companyId, long creatorUserId, List<WorkspaceMember> workspaceMembers)
        {
            Name = name;
            ImageUrl = imageUrl;
            Color = color;
            WorkspaceMembers = workspaceMembers;
            CreatorUserId = creatorUserId;
            CompanyId = companyId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void UpdateWorkspace(string name, string imageUrl, string color)
        {
            Name = name;
            ImageUrl = imageUrl;
            Color = color;
        }
        public void DeleteWorkspace()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        #endregion
    }
}

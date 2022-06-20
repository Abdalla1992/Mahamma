using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Workspace.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Workspace.Repository
{
   public interface IWorkspaceMemberRepository : IRepository<Entity.WorkspaceMember>
    {
        Task<List<WorkspaceMember>> GetWorkSpaceMemberById(int WorkSpaceId);
        Task<WorkspaceMember> GetByWorkspaceIdAndUserId(int workspaceId, long userId);
        void AddWorkSpaceMember(WorkspaceMember workSpaceMember);
        void DeleteWorkSpaceMember(WorkspaceMember workSpaceMember);
        void UpdateWorkSpaceMemberList(List<WorkspaceMember> workSpaceMember);
    }
}

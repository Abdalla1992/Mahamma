using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Task.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Repository
{
    public interface IMemberSearchReadRepository
    {
        Task<List<MemberDto>> SearchForMemerToAssignToWorkspace(string name, int companyId, long currentUserId, int workspaceId);
        Task<List<MemberDto>> SearchForMemerToAssignToProject(string name, int companyId, long currentUserId, int projectId);
        Task<List<MemberDto>> SearchForMemerToAssignToTask(string name, int companyId, long currentUserId, int taskId);
    }
}

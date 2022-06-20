using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Workspace.Dto;
using System.Threading.Tasks;

namespace Mahamma.Domain.Workspace.Repository
{
    public interface IWorkspaceRepository : IRepository<Entity.Workspace>
    {
        void AddWorkspace(Entity.Workspace workspace);
        Task<bool> CheckWorkspaceExistence(string name, int companyId, int id = default);
        Task<PageList<WorkspaceDto>> GetWorkspaceData(SearchWorkspaceDto searchWorkspaceDto, string role, string superAdminRole, long cuurentUserId, int companyId);
        Task<WorkspaceDto> GetById(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId);
        Task<Entity.Workspace> GetWorkspaceById(int id);
        void UpdateWorkspace(Entity.Workspace workspace);
        Task<bool> CheckWorkspaceIsFirstInCompany(int companyId);
        Task<bool> CheckWorkspaceIsInCompany(int workspaceId, int companyId);
    }
}

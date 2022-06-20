using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Repositroy
{
    public interface IProjectRepository : IRepository<Entity.Project>
    {
        void AddProject(Entity.Project project);
        Task<bool> CheckProjectExistence(string name, int workspaceId, int id = default);
        Task<PageList<ProjectUserDto>> GetProjectData(SearchProjectDto searchProjectDto, string role, string superAdminRole,long currentUserId,int companyId);
        Task<ProjectDto> GetById(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId, bool requestedFromMeeting = false);
        //this method to get project profile
        Task<ProjectDto> GetProjectDataById(int Id);
        Task<Entity.Project> GetProjectById(int id);
        Task<Entity.Project> GetProject(int id, string includeProperties = "");
        void UpdateProject(Entity.Project project);
        Task<bool> ValidDate(DateTime startDate, DateTime dueDate, int projectId);
        Task<List<ProjectDto>> GetProjectListByUser(long userId);
        Task<List<ProjectDto>> GetProjectListByCompanyId(int companyId);
        bool CheckCommentLikedByCurrentUser(int commentId, long userId);
    }
}

using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Repositroy
{
    public interface IProjectMemberRepository : IRepository<Entity.ProjectMember>
    {
        Task<bool> CheckMemberExistance(int projectId, long userId);

        Task<int> GetMemberById(int projectId, long userId);
        Task<Entity.ProjectMember> GetProjectMemberById(int projectMemberId);

        void AddProjectMember(Entity.ProjectMember projectmember);

        Task<List<Entity.ProjectMember>> GetProjectMemberExceptUserList(int projectId, List<long> userIds);
        void UpdateProjectMemberList(List<Entity.ProjectMember> projectMembers);
        void AddProjectMemberList(List<Entity.ProjectMember> projectMembers);
        Task<List<Entity.ProjectMember>> GetProjectMemberByProjectId(int projectId);
        Task<ProjectMemberDto> GetMemberByIdForMakeComment(int projectId, long userId);
        Task<ProjectMember> GetProjectCommentId(int projectMemberId);
        Task<List<ProjectMember>> GetProjectMemberByProjectIdExceptCurrentUser(int projectId, long currentUser);


    }
}

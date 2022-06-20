using AutoMapper;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectMemberRepository : Base.EntityRepository<ProjectMember>, IProjectMemberRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public ProjectMemberRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }


        public async Task<bool> CheckMemberExistance(int projectId, long userId)
        {
            return await GetAnyAsync(m => m.ProjectId == projectId && m.UserId == userId);

        }

        public void AddProjectMember(ProjectMember projectmember)
        {
            CreateAsyn(projectmember);
        }

        public async Task<List<ProjectMember>> GetProjectMemberExceptUserList(int projectId, List<long> userIds)
        {
            return await GetWhereAsync(m => m.ProjectId == projectId && !userIds.Contains(m.UserId));
        }
        public void UpdateProjectMemberList(List<ProjectMember> projectMembers)
        {
            UpdateList(projectMembers);
        }
        public void AddProjectMemberList(List<ProjectMember> projectMembers)
        {
            CreateListAsyn(projectMembers);
        }

        public async Task<List<ProjectMember>> GetProjectMemberByProjectId(int ProjectId)
        {
            return await GetWhereAsync(m => m.ProjectId == ProjectId);
        }



        public async Task<int> GetMemberById(int projectId, long userId)
        {
            int projectMemberId = default;
           ProjectMember  pro = await FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);
            if (pro != null)
            {
                projectMemberId = pro.Id;
            }
            return projectMemberId;
        }
        public async Task<ProjectMember> GetProjectMemberById(int projectMemberId)
        {
            return await FirstOrDefaultAsync(p => p.Id == projectMemberId && p.DeletedStatus != DeletedStatus.Deleted.Id);
        }

        public async  Task<ProjectMemberDto> GetMemberByIdForMakeComment(int projectId , long userId)
        {
            var member =await FirstOrDefaultAsync(p => p.ProjectId ==projectId && p.UserId == userId);
            return Mapper.Map<ProjectMemberDto>(member);
        }

        public async Task<ProjectMember> GetProjectCommentId(int projectMemberId)
        {
            return await FirstOrDefaultAsync(t => t.Id == projectMemberId);
        }

        public async Task<List<ProjectMember>> GetProjectMemberByProjectIdExceptCurrentUser(int projectId, long currentUser)
        {
            return await GetWhereAsync(m => m.ProjectId == projectId && m.UserId != currentUser);
        }
    }
}

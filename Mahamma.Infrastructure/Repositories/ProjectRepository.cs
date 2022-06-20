
using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectRepository : Base.EntityRepository<Project>, IProjectRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public ProjectRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }
        public void AddProject(Project project)
        {
            CreateAsyn(project);
        }
        public async Task<bool> CheckProjectExistence(string name, int workspaceId, int id = default)
        {
            return await GetAnyAsync(w => w.Name == name && w.WorkSpaceId == workspaceId && (id == default || w.Id != id) && w.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
        public async Task<PageList<ProjectUserDto>> GetProjectData(SearchProjectDto searchProjectDto, string role, string superAdminRole, long cuurentUserId, int companyId)
        {
            #region Declare Return Var with Intial Value

            PageList<ProjectUserDto> ProjectListDto = new();
            #endregion

            #region Preparing Filter
            Expression<Func<Project, bool>> filter;
            if (role == superAdminRole)
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                 && t.Workspace.CompanyId == companyId
                                                 && (searchProjectDto.Filter.WorkSpaceId > 0 ? t.WorkSpaceId == searchProjectDto.Filter.WorkSpaceId : t.Workspace.CompanyId == companyId)
                                                 && (string.IsNullOrEmpty(searchProjectDto.Filter.Name) || t.Name.ToLower().Contains(searchProjectDto.Filter.Name))
                                                 && (!searchProjectDto.Filter.DueDate.HasValue || t.DueDate.Date == searchProjectDto.Filter.DueDate.Value.Date);
            }
            else
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id 
                                                 && searchProjectDto.Filter.WorkSpaceId > 0 ? t.WorkSpaceId == searchProjectDto.Filter.WorkSpaceId : t.ProjectMembers.Any(m => m.UserId == cuurentUserId && m.DeletedStatus != DeletedStatus.Deleted.Id)
                                                 && t.ProjectMembers.Any(m => m.UserId == cuurentUserId && m.DeletedStatus != DeletedStatus.Deleted.Id)
                                                 && (string.IsNullOrEmpty(searchProjectDto.Filter.Name) || t.Name.ToLower().Contains(searchProjectDto.Filter.Name))
                                                 && (!searchProjectDto.Filter.DueDate.HasValue || t.DueDate.Date == searchProjectDto.Filter.DueDate.Value.Date);
            }

            #endregion

            List<Project> projectList = searchProjectDto.Sorting.Column switch
            {
                "name" => await GetPageAsyncWithoutQueryFilter(searchProjectDto.Paginator.Page, searchProjectDto.Paginator.PageSize, filter, x => x.Name, searchProjectDto.Sorting.SortingDirection.Id),
                _ => await GetPageAsyncWithoutQueryFilter(searchProjectDto.Paginator.Page, searchProjectDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Descending.Id),

            };
            if (projectList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                ProjectListDto.SetResult(totalCount, Mapper.Map<List<Project>, List<ProjectUserDto>>(projectList));
            }

            return ProjectListDto;
        }
        public async Task<ProjectDto> GetById(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId, bool requestedFromMeeting = false)
        {
            if (currentUserRole == superAdminRoleName || requestedFromMeeting)
            {
                return Mapper.Map<ProjectDto>(await FirstOrDefaultAsync(ll => ll.Id == id
                    && ll.Workspace.CompanyId == companyId && ll.DeletedStatus == DeletedStatus.NotDeleted.Id, "ProjectMembers.ProjectComments,ProjectMembers.ProjectComments.Replies,ProjectMembers.ProjectComments.Likes"));
            }
            else
            {
                return Mapper.Map<ProjectDto>(await FirstOrDefaultAsync(ll => ll.Id == id
                    && ll.ProjectMembers.Any(m => m.UserId == userId && m.DeletedStatus == DeletedStatus.NotDeleted.Id), "ProjectMembers.ProjectComments,ProjectMembers.ProjectComments.Replies,ProjectMembers.ProjectComments.Likes"));
            }
        }
        public async Task<Project> GetProjectById(int id)
        {
            return await FirstOrDefaultAsync(ll => ll.Id == id);
        }
        public async Task<Project> GetProject(int id, string includeProperties = "")
        {
            return await FirstOrDefaultAsyncWithoutQueryFilter(ll => ll.Id == id, includeProperties);
        }
        public void UpdateProject(Project project)
        {
            Update(project);
        }

        public async Task<ProjectDto> GetProjectDataById(int Id)
        {
            Domain.Project.Entity.Project project = await FirstOrDefaultAsync(t => t.Id == Id, "ProjectMembers,ProjectAttachments,Tasks"); //,Attachments,Tasks
            return Mapper.Map<ProjectDto>(project);
        }
        public async Task<bool> ValidDate(DateTime startDate, DateTime dueDate, int id)
        {
            Project project = await GetProjectById(id);
            if (project == null)
            {
                return false;
            }
            return project.DueDate >= dueDate;
        }

        public async Task<List<ProjectDto>> GetProjectListByUser(long userId)
        {
            List<Project> projectslist = await GetWhereAsync(p => p.ProjectMembers.Select(p => p.UserId).Contains(userId), "ProjectMembers,ProjectComments");
            return Mapper.Map<List<ProjectDto>>(projectslist);
        }
        public async Task<List<ProjectDto>> GetProjectListByCompanyId(int companyId)
        {
            List<Project> projectslist = await GetWhereAsync(p => p.Workspace.CompanyId == companyId, "Workspace");
            return Mapper.Map<List<ProjectDto>>(projectslist);
        }

        //public async Task<bool> CheckWorkSpaceValid(int projectId,int workspaceId, int id = default)
        //{
        //    return await GetAnyAsync(w => w.Id == projectId && w.WorkSpaceId >0 && (id == default || w.Id != id) && w.DeletedStatus == DeletedStatus.NotDeleted.Id);
        //}
        public bool CheckCommentLikedByCurrentUser(int commentId, long userId)
        {
            return AppDbContext.Set<ProjectLikeComment>().Any(c => c.ProjectCommentId == commentId && c.ProjectMember.UserId == userId);
        }
    }
}

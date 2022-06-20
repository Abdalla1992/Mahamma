using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Workspace.Dto;
using Mahamma.Domain.Workspace.Entity;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class WorkspaceRepository : Base.EntityRepository<Workspace>, IWorkspaceRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public WorkspaceRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }
        public void AddWorkspace(Workspace workspace)
        {
            CreateAsyn(workspace);
        }
        public async Task<bool> CheckWorkspaceExistence(string name, int companyId, int id = default)
        {
            return await GetAnyAsync(w => w.Name == name && w.CompanyId == companyId && (id == default || w.Id != id) && w.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
        public async Task<PageList<WorkspaceDto>> GetWorkspaceData(SearchWorkspaceDto searchWorkspaceDto, string role, string superAdminRole, long cuurentUserId, int companyId)
        {
            #region Declare Return Var with Intial Value
            PageList<WorkspaceDto> workspaceListDto = new();
            #endregion
            #region Preparing Filter 
            Expression<Func<Workspace, bool>> filter;
            if (role == superAdminRole)
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                          && t.CompanyId == companyId
                                                          && (string.IsNullOrEmpty(searchWorkspaceDto.Filter.Name) || t.Name.ToLower().Contains(searchWorkspaceDto.Filter.CleanName));
            }
            else
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                         && t.CompanyId == companyId
                                                         && t.WorkspaceMembers.Any(m => m.UserId == cuurentUserId && m.DeletedStatus != DeletedStatus.Deleted.Id)
                                                         && (string.IsNullOrEmpty(searchWorkspaceDto.Filter.Name) || t.Name.ToLower().Contains(searchWorkspaceDto.Filter.CleanName));
            }
            #endregion


            List<Workspace> workspaceList = searchWorkspaceDto.Sorting.Column switch
            {
                "name" => await GetPageAsyncWithoutQueryFilter(searchWorkspaceDto.Paginator.Page, searchWorkspaceDto.Paginator.PageSize, filter, x => x.Name, searchWorkspaceDto.Sorting.SortingDirection.Id),
                _ => await GetPageAsyncWithoutQueryFilter(searchWorkspaceDto.Paginator.Page, searchWorkspaceDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Ascending.Id),
            };
            if (workspaceList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                workspaceListDto.SetResult(totalCount, Mapper.Map<List<Workspace>, List<WorkspaceDto>>(workspaceList));
            }
            return workspaceListDto;
        }
        public async Task<WorkspaceDto> GetById(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId)
        {
            if (currentUserRole == superAdminRoleName)
            {
                return Mapper.Map<WorkspaceDto>(await FirstOrDefaultNoTrackingAsync(ll => ll.Id == id
                        && ll.CompanyId == companyId));
            }
            else
            {
                return Mapper.Map<WorkspaceDto>(await FirstOrDefaultNoTrackingAsync(ll => ll.Id == id
                        && ll.WorkspaceMembers.Any(m => m.UserId == userId && m.DeletedStatus == DeletedStatus.NotDeleted.Id)));
            }
        }

        public async Task<Workspace> GetWorkspaceById(int id)
        {
            return await FirstOrDefaultAsync(ll => ll.Id == id);
        }

        public void UpdateWorkspace(Workspace workspace)
        {
            Update(workspace);
        }
        public async Task<bool> CheckWorkspaceIsFirstInCompany(int companyId)
        {
            return !(await GetAnyAsync(w => w.CompanyId == companyId));
        }

        public async Task<bool> CheckWorkspaceIsInCompany(int workspaceId, int companyId)
        {
            return await GetAnyAsync(w => w.Id == workspaceId && w.CompanyId == companyId);
        }
    }
}

using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Role.Dto;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Infrastructure.Base;
using Mahamma.Identity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class RoleRepository : EntityRepository<Role>, IRoleRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public RoleRepository(IMapper mapper, IdentityContext context) : base(context, mapper)
        { }

        public void AddRole(Role role)
        {
            CreateAsyn(role);
        }

        public async Task<bool> CheckRoleExistence(string name, int id = default)
        {
            return await GetAnyAsync(m => m.Name == name && (id == default || m.Id != id) && m.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }

        public async Task<Role> GetRoleById(long roleId)
        {
            return await FirstOrDefaultAsync(m => m.Id == roleId);
        }

        public void RemoveRole(Role role)
        {
            Delete(role);
        }

        public async Task<PageList<RoleDto>> GetRoleData(SearchRoleDto searchRoleDto)
        {
            #region Declare Return Var with Intial Value

            PageList<RoleDto> RoleListDto = new();
            #endregion

            #region Preparing Filter
            Expression<Func<Role, bool>> filter = t => t.DeletedStatus != DeletedStatus.Deleted.Id
                                         && (String.IsNullOrEmpty(searchRoleDto.Filter.Name) || t.Name.ToLower().Contains(searchRoleDto.Filter.CleanName.ToLower()));

            #endregion

            List<Role> roleList = searchRoleDto.Sorting.Column switch
            {
                "name" => await GetPageAsyncWithoutQueryFilter(searchRoleDto.Paginator.Page, searchRoleDto.Paginator.PageSize, filter, x => x.Name, searchRoleDto.Sorting.SortingDirection.Id, "AspNetUserRoles.AspNetUsers"),
                _ => await GetPageAsyncWithoutQueryFilter(searchRoleDto.Paginator.Page, searchRoleDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Descending.Id, "AspNetUserRoles.AspNetUsers"),

            };
            if (roleList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                RoleListDto.SetResult(totalCount, Mapper.Map<List<Role>, List<RoleDto>>(roleList));
            }

            return RoleListDto;
        }

        public void UpdateRole(Role role)
        {
            Update(role);
        }

        public async Task<List<RoleDto>> GetAllCompanyRoles(int companyId)
        {
            var roleList = await GetWhereAsync(r => r.CompanyId == companyId);
            return Mapper.Map<List<RoleDto>>(roleList);
        }

        public async Task<RoleDto> GetRole(int roleId)
        {
            Role role = await FirstOrDefaultAsync(r => r.Id == roleId, "RolePagePermissions,RolePagePermissions.PagePermission");
            return Mapper.Map<RoleDto>(role);
        }

        public async Task<Role> GetRoleByNameAndCompany(string roleName, int companyId)
        {
            return await FirstOrDefaultAsync(r => r.Name == roleName && r.CompanyId == companyId);
        }

        //public async Task<List<RoleDto>> SetCompanyRoles(int companyId, Role role)
        //{
        //    var roleList = await GetWhereAsync(r => r.CompanyId == companyId);
        //    return Mapper.Map<List<RoleDto>>(roleList);
        //}
    }
}

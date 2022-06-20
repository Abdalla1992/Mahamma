using AutoMapper;
using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Infrastructure.Base;
using Mahamma.Identity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class RolePagePermissionRepository : EntityRepository<RolePagePermission>, IRolePagePermissionRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public RolePagePermissionRepository(IMapper mapper, IdentityContext context) : base(context, mapper)
        { }

        public async Task<bool> CheckRolePermission(long roleId, int pagePermissionId)
        {
            return await GetAnyAsync(r => r.RoleId == roleId && r.PagePermissionId == pagePermissionId && r.DeletedStatus != DeletedStatus.Deleted.Id);
        }
        public async Task<List<int>> GetIdsByRoleId(long roleId)
        {
            return (await GetWhereAsync(r => r.RoleId == roleId))?.Select(p => p.PagePermissionId).ToList();
        }

        public async Task<List<RolePagePermission>> GetPagepermissionByRoleId(long roleId)
        {
            return await GetWhereAsync(m => m.RoleId == roleId);
        }

        public void AddPagePermissionRole(RolePagePermission rolePagePermission)
        {
            CreateAsyn(rolePagePermission);
        }

        public void RemovePermissionList(List<RolePagePermission> deletedPermission)
        {
            DeleteList(deletedPermission);
        }


    }
}

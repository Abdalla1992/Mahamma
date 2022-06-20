using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Role.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Repository
{
    public interface IRolePagePermissionRepository : IRepository<Entity.RolePagePermission>
    {
        Task<bool> CheckRolePermission(long roleId, int pagePermissionId);
        Task<List<int>> GetIdsByRoleId(long roleId);
        Task<List<RolePagePermission>> GetPagepermissionByRoleId(long roleId);
        public void RemovePermissionList(List<RolePagePermission> deletedPermission);
        public void AddPagePermissionRole(RolePagePermission rolePagePermission);



    }
}

using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Role.Dto;
using Mahamma.Identity.Domain.Role.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Repository
{
    public interface IPagePermissionRepository : IRepository<Entity.PagePermission>
    {
        Task<int> GetIdByPageIdAndPermissionId(int pageId, int permissionId);
        Task<List<int>> GetPermissionsIds(List<int> pagePermissionId);
        Task<List<PagePermissionDto>> GetAllPagePermissions();
        Task<List<PagePermission>> GetPagePermissionsByPermissionIds(List<int> permissionIds);
        Task<List<int>> GetPagePermissionsIdsByPermissionId(int permissionId);
        Task<List<PagePermissionDto>> GetAllPagePermissionsToSetCompanyBasicRole();

    }
}

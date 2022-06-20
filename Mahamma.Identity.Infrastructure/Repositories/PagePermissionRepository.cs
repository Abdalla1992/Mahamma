using AutoMapper;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Role.Dto;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Enum;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Infrastructure.Base;
using Mahamma.Identity.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class PagePermissionRepository : EntityRepository<PagePermission>, IPagePermissionRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public PagePermissionRepository(IMapper mapper, IdentityContext context) : base(context, mapper)
        { }

        public async Task<int> GetIdByPageIdAndPermissionId(int pageId, int permissionId)
        {
            PagePermission pagePermission = await FirstOrDefaultAsync(p => p.PageId == pageId && p.PermissionId == permissionId);
            return pagePermission != null ? pagePermission.Id : default;
        }
        public async Task<List<int>> GetPermissionsIds(List<int> pagePermissionId)
        {
            return (await GetWhereAsync(p => pagePermissionId.Contains(p.Id))).Select(pp => pp.PermissionId).ToList();
        }

        public async Task<List<PagePermissionDto>> GetAllPagePermissions()
        {
            var addMeetingId = Permission.AddMeeting.Id;
            //return Mapper.Map<List<PagePermissionDto>>(await GetWhereAsync(null, "Page"));
            return Mapper.Map<List<PagePermissionDto>>(await GetWhereAsync(p => p.PermissionId != addMeetingId, "Page"));
        }
        public async Task<List<PagePermissionDto>> GetAllPagePermissionsToSetCompanyBasicRole()
        {
            return Mapper.Map<List<PagePermissionDto>>(await GetWhereAsync(null, "Page"));
        }
        public async Task<List<PagePermission>> GetPagePermissionsByPermissionIds(List<int> permissionIds)
        {
            return await GetWhereAsync(p => permissionIds.Contains(p.PermissionId));
        }
        public async Task<List<int>> GetPagePermissionsIdsByPermissionId(int permissionId)
        {
            return (await GetWhereAsync(p => p.PermissionId == permissionId)).Select(s => s.Id).ToList();
        }
    }
}

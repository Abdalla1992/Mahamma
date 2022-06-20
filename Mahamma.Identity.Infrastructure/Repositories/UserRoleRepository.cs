using AutoMapper;
using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.UserRole.Entity;
using Mahamma.Identity.Domain.UserRole.Repository;
using Mahamma.Identity.Infrastructure.Base;
using Mahamma.Identity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class UserRoleRepository : EntityRepository<UserRole>, IUserRoleRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public UserRoleRepository(IMapper mapper, IdentityContext context) : base(context, mapper)
        { }
        public async Task<long> GetRoleIdByUserId(long userId)
        {
            UserRole userRole = await FirstOrDefaultAsync(r => r.UserId == userId && r.DeletedStatus != DeletedStatus.Deleted.Id);
            return userRole != null ? userRole.RoleId : default;
        }

        public async Task<List<UserRole>> GetUsersByRoleId(long roleId)
        {
            return await GetWhereAsync(m => m.RoleId == roleId);
        }

        public void AddUserRole(UserRole userRole)
        {
            CreateAsyn(userRole);
        }

        public void UpdateUserRoleList(List<UserRole> RoleUsers)
        {
            UpdateList(RoleUsers);
        }

        public void RemoveUserRolee(UserRole RemoveRoleUsers)
        {
            Delete(RemoveRoleUsers);
        }

        public void RemovePermissionRoleList(List<UserRole> usersRolesList)
        {
            DeleteList(usersRolesList);
        }

        public async Task<UserRole> GetUserRoleByRoleIdAndUserId(long userId,long roleId)
        {
            return await FirstOrDefaultNoTrackingAsync(m => m.UserId == userId && m.RoleId == roleId);
        }

        public async Task<UserRole> GetUserRoleByUserId(long userId)
        {
            return await FirstOrDefaultNoTrackingAsync(m => m.UserId == userId);
        }

    }
}

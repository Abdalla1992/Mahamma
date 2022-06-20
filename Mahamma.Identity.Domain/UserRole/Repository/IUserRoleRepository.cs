using Mahamma.Identity.Domain._SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.UserRole.Repository
{
    public interface IUserRoleRepository : IRepository<Entity.UserRole>
    {
        Task<long> GetRoleIdByUserId(long userId);
        Task<List<Domain.UserRole.Entity.UserRole>> GetUsersByRoleId(long roleId);
        void AddUserRole(Entity.UserRole userRole);
        void UpdateUserRoleList(List<Entity.UserRole> RoleUsers);
        //void RemoveUserRole(List<Entity.UserRole> RemoveRoleUsers);
        void RemoveUserRolee(Entity.UserRole RemoveRoleUsers);
        void RemovePermissionRoleList(List<Entity.UserRole> usersRolesList);
        Task<Entity.UserRole> GetUserRoleByRoleIdAndUserId(long userId, long roleId);
        Task<Entity.UserRole> GetUserRoleByUserId(long userId);


    }
}

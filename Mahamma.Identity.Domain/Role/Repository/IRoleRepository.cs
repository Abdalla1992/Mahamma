using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Role.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Repository
{
   public interface IRoleRepository : IRepository<Entity.Role>
    {
        public void AddRole(Entity.Role role);
        Task<bool> CheckRoleExistence(string name , int id=default);
        Task <Domain.Role.Entity.Role> GetRoleById(long roleId);
        void RemoveRole(Entity.Role role);
        Task<PageList<RoleDto>> GetRoleData(SearchRoleDto request);
        void UpdateRole(Entity.Role role);
        Task<List<RoleDto>> GetAllCompanyRoles(int companyId);
        Task<Entity.Role> GetRoleByNameAndCompany(string roleName, int companyId);

        Task<RoleDto> GetRole(int roleId);
    }
}

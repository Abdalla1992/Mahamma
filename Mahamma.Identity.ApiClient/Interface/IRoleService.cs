using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.ApiClient.Interface
{
    public interface IRoleService
    {
        Task<bool> AuthorizeUser(UserPermissionDto userPermissionDto);
        Task<bool> SetCompanyBasicRoles(BaseRequestDto baseRequest, int companyId);
    }
}

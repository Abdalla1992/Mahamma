using Mahamma.Identity.Domain.PagePermissionByRoleId.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.PagePermissionByRoleId.Repository
{
    public interface IPagePermissionLocalizationRepository
    {
        Task<List<PagePermissionLocalizationDto>> GetPagePermissionByRoleId(long currentUserId , long currentRoleId);

    }
}


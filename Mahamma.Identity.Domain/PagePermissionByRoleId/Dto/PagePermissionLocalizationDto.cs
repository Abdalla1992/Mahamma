using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.PagePermissionByRoleId.Dto
{
    public class PagePermissionLocalizationDto
    {
        public int PagePermissionId { get; set; }
        public string PageName { get; set; }
        public string PermissionName { get; set; }
        public long RoleId { get; set; }
        public bool IsAssigned { get; set; }

    }
}

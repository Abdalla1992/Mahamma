using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.ApiClient.Dto.Role
{
    public class UserPermissionDto
    {
        public long UserId { get; set; }
        public int PermissionId { get; set; }
        public int PageId { get; set; }
    }
}

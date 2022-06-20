using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Entity
{
    public class PagePermission : Entity<int>, IAggregateRoot
    {
        public int PageId { get; set; }
        public int PermissionId { get; set; }
        public List<RolePagePermission> RolePagePermissions { get; set; }
        public Page Page { get; set; }

        public PagePermission(int id, int pageId, int permissionId)
        {
            Id = id;
            PageId = pageId;
            PermissionId = permissionId;
            CreationDate = new DateTime(2021, 10, 19);
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
    }
}

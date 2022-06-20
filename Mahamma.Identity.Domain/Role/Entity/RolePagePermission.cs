using Mahamma.Base.Domain;
using System;

namespace Mahamma.Identity.Domain.Role.Entity
{
    public class RolePagePermission : Entity<int>, IAggregateRoot
    {
        public long RoleId { get; set; }
        public int PagePermissionId { get; set; }
        public PagePermission PagePermission { get; set; }

        public void CreatRolePagePermission(int pagePermissionId, long roleId)
        {
            RoleId = roleId;
            PagePermissionId = pagePermissionId;

            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        //public int PagePermissionId { get; set; }

    }
}

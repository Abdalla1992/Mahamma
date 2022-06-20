using Mahamma.Base.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.UserRole.Entity
{
    public class UserRole : IdentityUserRole<long>, IAggregateRoot
    {
        public DateTime CreationDate { get; set; }
        public int DeletedStatus { get; set; }

        public void CreatUserRoles(long usersIds, long roleId)
        {
            RoleId = roleId;
            UserId = usersIds;

            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }


        public void DeleteUserRole()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

      
    }
}

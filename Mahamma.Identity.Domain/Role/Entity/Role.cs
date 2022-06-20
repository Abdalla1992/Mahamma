using Mahamma.Base.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Entity
{
    public class Role : IdentityRole<long>, IAggregateRoot
    {
        public DateTime CreationDate { get; set; }
        public int DeletedStatus { get; set; }
        public int? CompanyId { get; set; }
        public List<RolePagePermission> RolePagePermissions { get; set; }


        public void CreateRole(string name, int companyId)
        {
            Name = name;
            NormalizedName = Name.ToUpper();
            CompanyId = companyId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void DeleteRole()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }


        public void UpdateRole(string roleName)
        {
            Name = roleName;
        }
    }
}

using Mahamma.Base.Dto.Dto;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.User.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Dto
{
     public class RoleDto  : BaseDto<int>
    {
        #region Prop
        public string Name { get; set; }
        public List<UserDto> Users { get; set; }
        public List<PagePermissionDto> PagePermissions { get; set; }
        #endregion

        #region Methods
        public string CleanName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return string.Empty;
                else
                    return RemoveWhiteSpace(Name.ToLower().Trim());
            }
        }
        #endregion
    }
}

using Mahamma.Base.Dto.Dto;
using Mahamma.Identity.Domain.Role.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Dto
{
    public class PagePermissionDto : BaseDto<int>
    {
        #region Prop
        public int PageId { get; set; }
        public int PermissionId { get; set; }
        public string PageName { get; set; }
        public string PermissionName { get; set; }
        public PageDto Page { get; set; }
        #endregion
    }
}

using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectActivity.Dto
{
   public class ProjectActivityDto : BaseDto<int>
    {
        #region PRop    
        public string Action { get; set; }
        public string MemberProfileImage { get; set; }
        public int ProjectMemberId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreationDate { get; set; }
        #endregion
    }
}

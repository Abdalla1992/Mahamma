using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.MemberSearch.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Workspace.Dto
{
    public class WorkspaceUserDto : BaseDto<int>
    {
        #region Props
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public int CompanyId { get; set; }
        public long CreatorUserId { get; set; }
        public List<long> UserIdList { get; set; }
        public List<MemberDto> Members { get; set; }
        #endregion
    }
}

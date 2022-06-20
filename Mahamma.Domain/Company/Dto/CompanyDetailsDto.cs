using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Company.Dto
{
    public class CompanyDetailsDto : BaseDto<int>
    {
        #region Props
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Color { get; set; }
        public long CreatorUserId { get; set; }
        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public string Image { get; set; }

        #endregion

    }
}

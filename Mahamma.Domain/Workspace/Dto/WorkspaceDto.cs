using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Workspace.Dto
{
    public class WorkspaceDto : BaseDto<int>
    {
        #region Props
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public int CompanyId { get; set; }
        public long CreatorUserId { get; set; }
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

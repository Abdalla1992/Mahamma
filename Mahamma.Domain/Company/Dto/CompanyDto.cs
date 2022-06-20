using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.ProjectAttachment.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Company.Dto
{
    public class CompanyDto : BaseDto<int>
    {
        #region Props
        public string Name { get; set; }
        public string CompanySize { get; set; }
        public string FolderPath { get; set; }
        public string Descreption { get; set; }
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

using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Language.Dto
{
    public class LanguageDto : BaseDto<int>
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public bool IsRtl { get; set; }
    }
}

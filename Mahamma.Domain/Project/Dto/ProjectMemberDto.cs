using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Dto
{
    public class ProjectMemberDto : BaseDto<int>
    {
        public long UserId { get; set; }
        public int ProjectId { get; set; }
        public double? Rating { get; set; }
    }
}

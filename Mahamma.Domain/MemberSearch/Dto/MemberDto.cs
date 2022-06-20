using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.MemberSearch.Dto
{   
    public class MemberDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public double? Rating { get; set; }
    }
}

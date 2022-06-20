using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.MemberSearch.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Dto
{
    public class ProjectCommentDto : BaseDto<int>
    {
        public string Comment { get; set; }
        public int ProjectMemberId { get; set; }
        public long? UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public int LikesCount { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public MemberDto Member { get; set; }
        public List<ProjectCommentDto> Replies { get; set; }
        public string CreationDuration { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }
}

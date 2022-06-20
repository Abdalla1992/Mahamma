using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.MemberSearch.Dto;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Task.Dto
{
    public class TaskCommentDto : BaseDto<int>
    {
        public string Comment { get; set; }
        public int TaskMemberId { get; set; }
        public long? UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public int LikesCount { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public MemberDto Member { get; set; }
        public List<TaskCommentDto> Replies { get; set; }
        public string CreationDuration { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }
}

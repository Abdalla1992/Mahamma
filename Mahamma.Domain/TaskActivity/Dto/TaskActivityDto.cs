using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.MemberSearch.Dto;
using System;

namespace Mahamma.Domain.TaskActivity.Dto
{
    public class TaskActivityDto : BaseDto<int>
    {
        public string Action { get; set; }
        public int TaskId { get; set; }
        public int TaskMemberId { get; set; }
        public long? UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public MemberDto Member { get; set; }
    }
}

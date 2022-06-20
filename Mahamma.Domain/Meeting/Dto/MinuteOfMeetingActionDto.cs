using Mahamma.Base.Domain;
using Mahamma.Domain.MemberSearch.Dto;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Dto
{
    public class MinuteOfMeetingActionDto
    {
        #region Prop
        public int Id { get; set; }
        public string ActionTitle { get; set; }
        public int ActionLevel { get; set; }
        public string Assignee { get; set; }
        public List<MemberDto> Members { get; set; }
        public int ProgressPercentage { get; set; }
        public bool IsDraft { get; set; }
        public int? WorkspaceId { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public int? ParentTaskId { get; set; }
        public string Description { get; set; }
        #endregion
    }
}

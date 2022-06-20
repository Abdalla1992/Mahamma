using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.ProjectAttachment.Dto;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Project.Dto
{
    public class ProjectUserDto : BaseDto<int>
    {
        #region Props
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int WorkSpaceId { get; set; }
        public long CreatorUserId { get; set; }
        public List<long> UserIdList { get; set; }
        public List<MemberDto> Members { get; set; }
        public List<ProjectAttachmentDto> ProjectAttachments{ get; set; }
        public List<ProjectCommentDto> ProjectComments { get; set; }

        public int FilesCount{ get; set; }
        public double ProgressPercentage { get; set; }
        public List<MinuteOfMeetingDto> ProjectMinuteOfMeetings { get; set; }
        #endregion
    }
}

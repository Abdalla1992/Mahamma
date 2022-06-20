using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.ProjectAttachment.Dto;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Task.Dto
{
    public class TaskDto : BaseDto<int>
    {
        #region Props
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int TaskPriorityId { get; set; }
        public bool ReviewRequest { get; set; }
        public int? ParentTaskId { get; set; }
        public string ParentTaskName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TaskStatusId { get; set; }
        public long CreatorUserId { get; set; }
        public int FilesCount { get; set; }
        public int WorkspaceId { get; set; }
        public double? Rating { get; set; }
        public double ProgressPercentage { get; set; }
        public DateTime? UpComingMeetingDate { get; set; }
        #endregion
        #region Nav Prop
        public List<ProjectAttachmentDto> TaskAttachments { get; set; }
        public List<TaskMemberDto> TaskMembers { get; set; }
        public List<TaskCommentDto> TaskComments { get; set; }
        public List<TaskDto> SubTasks { get; set; }
        public List<MemberDto> Members { get; set; }
        public List<MinuteOfMeetingDto> TaskMinuteOfMeetings { get; set; }
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

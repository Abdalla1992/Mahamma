using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.Meeting.Dto;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Project.Dto
{
    public class ProjectDto :BaseDto<int>
    {
        #region Props
        public string Name { get; set; }
        public string Description { get; set; } 
        public DateTime? DueDate { get; set; }
        public int WorkSpaceId { get; set; }
        public long CreatorUserId { get; set; }
        public double ProgressPercentage { get; set; }
        public DateTime? UpComingMeetingDate { get; set; }
        #endregion

        #region Navigation Prop
        public List<ProjectCommentDto> ProjectComments { get; set; }





        //public List<ProjectMember> ProjectMembers { get; set; }
        //public List<Mahamma.Domain.Task.Entity.Task> Tasks { get; set; }
        //public List<ProjectAttachment.Entity.ProjectAttachment> ProjectAttachments { get; set; }
        //public List<ProjectActivity.Entity.ProjectActivity> ProjectActivities { get; set; }
        public List<MinuteOfMeetingDto> ProjectMinuteOfMeetings { get; set; }

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

using Mahamma.Base.Domain;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using System;

using System.Collections.Generic;

namespace Mahamma.Domain.Project.Entity
{
    public class Project : Entity<int>, IAggregateRoot
    {
        #region Props
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int WorkSpaceId { get; set; }
        public long CreatorUserId { get; set; }
        public double ProgressPercentage { get; set; }
        #endregion

        #region Navigation Prop
        public Domain.Workspace.Entity.Workspace Workspace { get; set; }
        public List<ProjectMember> ProjectMembers { get; set; }
        public List<ProjectComment> ProjectComments { get; set; }
        public List<Mahamma.Domain.Task.Entity.Task> Tasks { get; set; }
        public List<ProjectAttachment.Entity.ProjectAttachment> ProjectAttachments { get; set; }
        public List<ProjectActivity.Entity.ProjectActivity> ProjectActivities { get; set; }
        public virtual ProjectCharter ProjectCharter { get; set; }
        public virtual List<ProjectRiskPlan> ProjectRiskPlans { get; set; }
        public virtual List<ProjectCommunicationPlan> ProjectCommunicationPlans { get; set; }
        #endregion

        #region Methods
        public void CreateProject(string name , string descreption , DateTime duedate , 
            int workspaceid,long creatorUserId, List<ProjectMember> projectMembers,double progressPercentage, bool? isCreatedFromMeeting = false)
        {
            Name = name;
            Description = descreption;
            DueDate = duedate;
            WorkSpaceId = workspaceid;
            CreatorUserId = creatorUserId;
            CreationDate = DateTime.Now;
            ProjectMembers = projectMembers;
            if (isCreatedFromMeeting ?? false)
            {
                DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
            }
            else
            {
                DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            }
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            ProjectMembers = projectMembers;
            ProgressPercentage = progressPercentage;
        }
        public void UpdateProject(string name, string descreption, DateTime duedate, int workspaceid)
        {
            Name = name;
            Description = descreption;
            DueDate = duedate;
            WorkSpaceId = workspaceid;
        }

        public void DeleteProject()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

        public void Activate()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void ArchiveProject()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Archived.Id;
        }
        #endregion

    }
}

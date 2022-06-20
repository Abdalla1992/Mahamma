using Mahamma.Base.Domain;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.Task.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mahamma.Domain.Task.Entity
{
    public class Task : Entity<int>, IAggregateRoot
    {
        #region Props
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int TaskPriorityId { get; set; }
        public bool ReviewRequest { get; set; }
        public int? ParentTaskId { get; set; }
        public int ProjectId { get; set; }
        public int TaskStatusId { get; set; }
        public long CreatorUserId { get; set; }
        public double? Rating { get; set; }
        public double ProgressPercentage { get; set; }
        public int? DependencyTaskId { get; set; }
        #endregion

        #region Navigation Properties
        public Project.Entity.Project Project { get; set; }
        public List<TaskMember> TaskMembers { get; set; }
        public List<TaskComment> TaskComments { get; set; }
        public List<Task> SubTask { get; set; }
        public List<ProjectAttachment.Entity.ProjectAttachment> Attachments { get; set; }
        public List<TaskActivity.Entity.TaskActivity> TaskActivities { get; set; }
        #endregion

        #region Methods
        public void CreateTask(int projectId, string name, string description, DateTime startDate, DateTime dueDate, int taskPriorityId,
         bool reviewRequest, int? parentTaskId, long creatorUserId, List<TaskMember> taskMemberList, double progressPercentage,
         bool? isCreatedFromMeeting = false, int? dependencyTaskId = default)
        {
            ProjectId = projectId;
            Name = name;
            Description = description;
            StartDate = startDate;
            DueDate = dueDate;
            TaskPriorityId = taskPriorityId;
            ReviewRequest = reviewRequest;
            ParentTaskId = parentTaskId;
            CreatorUserId = creatorUserId;
            CreationDate = DateTime.Now;
            TaskMembers = taskMemberList;
            TaskStatusId = TaskStatus.New.Id;
            if (isCreatedFromMeeting ?? false)
            {
                DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
            }
            else
            {
                DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            }
            ProgressPercentage = progressPercentage;
            DependencyTaskId = dependencyTaskId;
        }
        public void UpdateTask(string name, string description, DateTime startDate, DateTime dueDate, int taskPriorityId, bool reviewRequest)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            DueDate = dueDate;
            TaskPriorityId = taskPriorityId;
            ReviewRequest = reviewRequest;
        }
        public void Activate()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void DeleteTask()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        public void ArchiveTask()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Archived.Id;
        }
        #endregion
    }
}


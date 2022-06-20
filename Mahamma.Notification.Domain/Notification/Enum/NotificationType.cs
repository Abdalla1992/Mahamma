using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Notification.Enum
{
    public class NotificationType : Enumeration
    {
        #region Enum Value
        public static NotificationType AddProject = new(1, nameof(AddProject));
        public static NotificationType ArchiveProject = new(2, nameof(ArchiveProject));
        public static NotificationType AssignMemberToProject = new(3, nameof(AssignMemberToProject));
        public static NotificationType DeleteProject = new(4, nameof(DeleteProject));
        public static NotificationType UpdateProject = new(5, nameof(UpdateProject));
        public static NotificationType AddComment = new(6, nameof(AddComment));
        public static NotificationType AddTask = new(7, nameof(AddTask));
        public static NotificationType ArchiveTask = new(8, nameof(ArchiveTask));
        public static NotificationType AssignMemberToTask = new(9, nameof(AssignMemberToTask));
        public static NotificationType DeleteTask = new(10, nameof(DeleteTask));
        public static NotificationType LikeComment = new(11, nameof(LikeComment));
        public static NotificationType SubmitTask = new(12, nameof(SubmitTask));
        public static NotificationType UpdateTask = new(13, nameof(UpdateTask));
        public static NotificationType AssignMemberToWorkspace = new(14, nameof(AssignMemberToWorkspace));
        public static NotificationType AddWorkspace = new(15, nameof(AddWorkspace));
        public static NotificationType UpdateWorkspace = new(16, nameof(UpdateWorkspace));
        public static NotificationType AddSubTask = new(17, nameof(AddSubTask));
        public static NotificationType AcceptedTask = new(18, nameof(AcceptedTask));
        public static NotificationType RejectTask = new(19, nameof(RejectTask));
        public static NotificationType MentionComment = new(20, nameof(MentionComment));
        public static NotificationType MeetingAdded = new(21, nameof(MeetingAdded));
        public static NotificationType MeetingUpdated = new(22, nameof(MeetingUpdated));
        public static NotificationType MeetingCanceled = new(23, nameof(MeetingCanceled));
        public static NotificationType UpdateSubTask = new(24, nameof(UpdateTask));
        public static NotificationType DeleteSubTask = new(25, nameof(DeleteSubTask));
        public static NotificationType TaskIsInProgress = new(26, nameof(TaskIsInProgress));
        public static NotificationType SubTaskIsInProgress = new(27, nameof(SubTaskIsInProgress));
        public static NotificationType MinuteOfMeetingAdded = new(28, nameof(MinuteOfMeetingAdded));

        #endregion

        #region Ctor
        public NotificationType(int id, string name) : base(id, name)
        {
        }
        #endregion


        #region Methods
        public static IEnumerable<NotificationType> List() => new[] { AddProject, ArchiveProject, AssignMemberToProject,
            DeleteProject, UpdateProject, AddComment, AddTask,ArchiveTask,AssignMemberToTask,DeleteTask,LikeComment,
            SubmitTask,UpdateTask,AssignMemberToWorkspace, AddWorkspace,UpdateWorkspace,AddSubTask, AcceptedTask,
            RejectTask, MentionComment, MeetingAdded, MeetingUpdated, MeetingCanceled, UpdateSubTask, DeleteSubTask,
            TaskIsInProgress, SubTaskIsInProgress, MinuteOfMeetingAdded};

        public static NotificationType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        public static NotificationType From(int id)
        {
            NotificationType state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for NotificationType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        #endregion

    }
}

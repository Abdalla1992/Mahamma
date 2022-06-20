using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahamma.Identity.Domain.Role.Enum
{
    public class Permission : Enumeration
    {
        #region Enum Values
        public static Permission AddWorkspace = new(1, nameof(AddWorkspace));
        public static Permission ViewWorkspace = new(2, nameof(ViewWorkspace));
        public static Permission UpdateWorkspace = new(3, nameof(UpdateWorkspace));
        public static Permission DeleteWorkspace = new(4, nameof(DeleteWorkspace));
        public static Permission ViewTask = new(5, nameof(ViewTask));
        public static Permission AddTask = new(6, nameof(AddTask));
        public static Permission UpdateTask = new(7, nameof(UpdateTask));
        public static Permission DeleteTask = new(8, nameof(DeleteTask));
        public static Permission ArchiveTask = new(9, nameof(ArchiveTask));
        public static Permission AddFile = new(10, nameof(AddFile));
        public static Permission ViewFile = new(11, nameof(ViewFile));
        public static Permission DeleteFile = new(12, nameof(DeleteFile));
        public static Permission AssignMember = new(13, nameof(AssignMember));
        public static Permission AddComment = new(14, nameof(AddComment));
        public static Permission AddProject = new(15, nameof(AddProject));
        public static Permission ViewProject = new(16, nameof(ViewProject));
        public static Permission UpdateProject = new(17, nameof(UpdateProject));
        public static Permission DeleteProject = new(18, nameof(DeleteProject));
        public static Permission ArchiveProject = new(19, nameof(ArchiveProject));
        public static Permission ViewRole = new(20, nameof(ViewRole));
        public static Permission AddRole = new(21, nameof(AddRole));
        public static Permission UpdateRole = new(22, nameof(UpdateRole));
        public static Permission DeleteRole = new(23, nameof(DeleteRole));
        public static Permission UploadDocument = new(24, nameof(UploadDocument));
        public static Permission DownloadDocument = new(25, nameof(DownloadDocument));
        public static Permission DeleteDocument = new(26, nameof(DeleteDocument));
        public static Permission SubmitTask = new(27, nameof(SubmitTask));
        public static Permission AddSubTask = new(28, nameof(AddSubTask));
        //public static Permission ViewCharts = new(29, nameof(ViewCharts));
        public static Permission AddMeetingGeneral = new(30, nameof(AddMeetingGeneral));
        public static Permission AddMeetingWorkspace = new(31, nameof(AddMeetingWorkspace));
        public static Permission AddMeetingProject = new(32, nameof(AddMeetingProject));
        public static Permission AddMeetingTask = new(33, nameof(AddMeetingTask));
        public static Permission UpdateMeeting = new(34, nameof(UpdateMeeting));
        public static Permission DeleteMeeting = new(35, nameof(DeleteMeeting));
        public static Permission ViewMeeting = new(36, nameof(ViewMeeting));
        //public static Permission UpdateMinuteOfMeeting = new(37, nameof(UpdateMinuteOfMeeting));
        //public static Permission DeleteMinuteOfMeeting = new(38, nameof(DeleteMinuteOfMeeting));
        //public static Permission AddMinuteOfMeeting = new(39, nameof(AddMinuteOfMeeting));
        //public static Permission ViewMinuteOfMeeting = new(40, nameof(ViewMinuteOfMeeting));
        public static Permission AddMeeting = new(41, nameof(AddMeeting));




        #endregion
        #region CTRS
        public Permission(int id, string name)
            : base(id, name)
        {
        }
        #endregion  

        #region Methods
        public static IEnumerable<Permission> List() => new[] { AddWorkspace, ViewWorkspace, UpdateWorkspace, DeleteWorkspace,
            ViewTask, AddTask, UpdateTask, DeleteTask, ArchiveTask, AddFile, ViewFile, DeleteFile, AssignMember, AddComment,
            AddProject, ViewProject, UpdateProject, DeleteProject, ArchiveProject, ViewRole, AddRole, UpdateRole,
            DeleteRole,UploadDocument,DownloadDocument,DeleteDocument,SubmitTask,AddSubTask,
            AddMeetingGeneral,AddMeetingWorkspace,AddMeetingProject,AddMeetingTask,UpdateMeeting,DeleteMeeting,AddMeeting,ViewMeeting };
        //ViewMeeting,UpdateMinuteOfMeeting,DeleteMinuteOfMeeting,AddMinuteOfMeeting,ViewMinuteOfMeeting,//ViewCharts

        public static IEnumerable<Permission> AddMeetingList() => new[] { AddMeetingGeneral, AddMeetingProject, AddMeetingWorkspace, AddMeetingTask };
        public static Permission FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for Permission: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static Permission From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for Permission: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion
    }
}

using Mahamma.Base.Domain;
using Mahamma.Domain.Meeting.Enum;
using System;

namespace Mahamma.Domain.Meeting.Entity
{
    public class MinuteOfMeeting : Entity<int>
    {
        #region Prop
        public int MeetingId { get; private set; }
        public string Description { get; private set; }
        public int? ProjectId { get; private set; }
        public int? TaskId { get; private set; }
        public bool IsDraft { get; private set; }
        public int MinuteOfMeetingLevel { get; private set; }
        public long CreatorUserId { get; private set; }
        #endregion

        #region Navigation
        //public  Domain.Task.Entity.Task Task { get; set; }
        #endregion

        public void CreateMinuteOfMeeting(int meetingId, string description, int? projectId, int? taskId, int minuteOfMeetingLevel, long creatorUserId, bool isDraft = false)
        {
            MeetingId = meetingId;
            Description = description;
            ProjectId = projectId;
            TaskId = taskId;
            IsDraft = isDraft;
            MinuteOfMeetingLevel = minuteOfMeetingLevel;
            CreatorUserId = creatorUserId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void UpdateMinuteOfMeeting(string description)
        {
            Description = description;
        }

        public void DeleteMinuteOfMeeting()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

        internal void Publish()
        {
            IsDraft = false;
        }
    }
}

using Mahamma.Base.Domain;
using System;

namespace Mahamma.Domain.Meeting.Entity
{
    public class MeetingAgendaTopics : Entity<int>
    {
        #region Prop
        public int MeetingId { get; private set; }
        public string Topic { get; private set; }
        public int DurationInMinutes { get; private set; }
        #endregion
         
        #region Navigation
        #endregion

        public void CreateMeetingTopic(int meetingId, string topic,int durationInMinutes)
        {
            Topic = topic;
            DurationInMinutes = durationInMinutes;
            MeetingId = meetingId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }


        public void DeleteMeetingTopic()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }

    }
}

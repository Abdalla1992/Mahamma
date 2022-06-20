using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahamma.Domain.Meeting.Entity
{
    public class Meeting : Entity<int>, IAggregateRoot
    {
        #region Props
        public string Title { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan Time { get; private set; }
        public int Duration { get; private set; }
        public int DurationUnitType { get; private set; }
        public int CompanyId { get; private set; }
        public int? WorkSpaceId { get; private set; }
        public int? ProjectId { get; private set; }
        public int? TaskId { get; private set; }
        public long CreatorUserId { get; private set; }
        public bool IsOnline { get; private set; }
        public string JoinUrl { get; private set; }
        public long ZoomMeetingId { get; private set; }

        #endregion

        #region Navigation Prop
        public List<MeetingMember> Members { get; set; }
        public List<MeetingAgendaTopics> AgendaTopics { get; private set; }
        public List<MinuteOfMeeting> MinutesOfMeeting { get; private set; }
        public List<MeetingFile> MeetingFiles { get; set; }
        #endregion

        #region Methods
        public void CreateMeeting(string title, DateTime date, TimeSpan time, int duration, int durationUnitType, int companyId, int? workSpaceId, int? projectId,
            int? taskId, long creatorUserId, List<MeetingAgendaTopics> agendaTopics,
            List<MeetingMember> meetingMember, bool isOnline,
            List<MeetingFile> meetingFiles)
        {
            Title = title;
            Date = date;
            Time = time;
            Duration = duration;
            DurationUnitType = durationUnitType;
            CompanyId = companyId;
            WorkSpaceId = workSpaceId;
            ProjectId = projectId;
            TaskId = taskId;
            CreatorUserId = creatorUserId;
            Members = meetingMember;
            AgendaTopics = agendaTopics;
            IsOnline = isOnline;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            MeetingFiles = meetingFiles;

        }
        public void UpdateMeeting(string title, DateTime date, TimeSpan time, int duration, int durationUnitType, List<MeetingAgendaTopics> agendaTopics, List<MeetingMember> meetingMember, bool isOnline,List<MeetingFile> meetingFiles)
        {
            if (Date > DateTime.Now)
            {
                Title = title;
                Date = date;
                Time = time;
                Duration = duration;
                DurationUnitType = durationUnitType;
                IsOnline = isOnline;
                AgendaTopics.ForEach(a => a.DeleteMeetingTopic());
                AgendaTopics.AddRange(agendaTopics);
                Members.ForEach(a => a.DeleteMeetingMember());
                Members.AddRange(meetingMember);
                MeetingFiles = meetingFiles;
            }
        }

        public void SubmitAttendance(List<long> attendees)
        {
            if (attendees.Count > 0)
            {
                foreach (var member in Members.Where(m => attendees.Contains(m.UserId)))
                {
                    member.HaveAttended();
                }
            }
        }

        public void PublicAllMinutesOfMeeting()
        {
            foreach (var minute in MinutesOfMeeting.Where(m => m.DeletedStatus == Base.Dto.Enum.DeletedStatus.NotDeleted.Id))
            {
                minute.Publish();
            }
        }

        public void DeleteMeeting(long userId)
        {
            if (Date > DateTime.Now && CreatorUserId == userId)
            {
                DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
            }
        }

        public bool AddMinuteOfMeeting(MinuteOfMeeting minuteOfMeeting)
        {
            if (Members.Any(m => m.UserId == minuteOfMeeting.CreatorUserId && m.CanMakeMinuteOfMeeting == true))
            {
                MinutesOfMeeting.Add(minuteOfMeeting);
                return true;
            }
            return false;
        }

        public void SetZoomMeetingData(string joinUrl, long zoomMeetingId)
        {
            JoinUrl = joinUrl;
            ZoomMeetingId = zoomMeetingId;
        }
        #endregion

    }
}

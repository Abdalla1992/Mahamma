using Mahamma.Base.Dto.Dto;
using Mahamma.Domain.MemberSearch.Dto;
using System;
using System.Collections.Generic;

namespace Mahamma.Domain.Meeting.Dto
{
    public class MeetingDto : BaseDto<int>
    {
        #region Props
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int Duration { get; set; }
        public int DurationUnitType { get; set; }
        public int CompanyId { get; set; }
        public int? WorkSpaceId { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public IDictionary<long, List<int>> MemberList { get; set; }
        public List<long> AttendanceIdList { get; set; }
        public List<AgendaTopicDto> AgendaTopics { get; set; }
        public List<MemberDto> Members { get; set; }
        public List<MinuteOfMeetingDto> MinutesOfMeeting { get; set; }
        public long CreatorUserId { get; set; }
        public bool IsOnline { get; set; }
        public string JoinUrl { get; set; }
        public List<MeetingFilesDto> MeetingFiles { get; set; }
        #endregion

        #region Navigation Prop
        #endregion

        #region Methods
        public string CleanName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Title))
                    return string.Empty;
                else
                    return RemoveWhiteSpace(Title.ToLower().Trim());
            }
        }
        #endregion
    }
}

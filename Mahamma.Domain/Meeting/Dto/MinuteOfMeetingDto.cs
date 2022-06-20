using Mahamma.Base.Domain;
using System;

namespace Mahamma.Domain.Meeting.Dto
{
    public class MinuteOfMeetingDto : Entity<int>
    {
        #region Prop
        public int MeetingId { get; set; }
        public string Description { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public bool IsDraft { get; set; }
        #endregion
    }
}

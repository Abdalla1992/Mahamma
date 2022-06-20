using System;

namespace Mahamma.Domain.MyWork.Dto
{
    public class UserTasksInfoDto
    {
        public int TasksCount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime LastUpateDate { get; set; }
        public int ProgressPercentage { get; set; }
        public DateTime UpcomingMeetingDate { get; set; }
    }
}

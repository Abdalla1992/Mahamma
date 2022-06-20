using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Dto
{
    public class UserProfileTaskHistoryDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public double CompletedTaskPercentage { get; set; }
        public double PendingTaskPercentage { get; set; }
        public double Rating { get; set; }
        public List<UserTaskDto> UserTasks { get; set; }
    }
}

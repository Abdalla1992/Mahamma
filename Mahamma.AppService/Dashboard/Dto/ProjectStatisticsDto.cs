using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Dashboard.Dto
{
    public class ProjectStatisticsDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public List<Tuple<string, int, string>> Tasks { get; set; }
        public List<Tuple<string, int, string>> CompletedTasksStatistics { get; set; }
        public List<Tuple<string, int, string>> NotCompletedTasksStatistics { get; set; }
        public List<Tuple<string, int, string>> SubTasks { get; set; }
        public List<Tuple<string, int, string>> CompletedSubTasksStatistics { get; set; }
        public List<Tuple<string, int, string>> NotCompletedSubTasksStatistics { get; set; }
    }
}
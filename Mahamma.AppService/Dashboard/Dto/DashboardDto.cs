using Mahamma.Domain.Task.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Dashboard.Dto
{
    public class DashboardDto
    {
        public ProjectStatisticsDto ProjectStatistics { get; set; }
        public List<TaskDto> Tasks { get; set; }
        public List<TaskDto> Subtasks { get; set; }
    }
}

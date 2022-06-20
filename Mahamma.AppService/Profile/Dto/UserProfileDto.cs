using Mahamma.Domain.Task.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Profile.Dto
{
    public class UserProfileDto
    {
        public string ProfilePic { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public double Rating { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int WorkingHours { get; set; }
        public string Bio { get; set; }
        public List<TaskDto> TasksHistory { get; set; }
    }
}

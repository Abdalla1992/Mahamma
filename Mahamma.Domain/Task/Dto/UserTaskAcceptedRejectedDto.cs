using Mahamma.Base.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Dto
{
    public class UserTaskAcceptedRejectedDto : BaseDto<int>
    {
        public string TaskName { get; set; }
        public string ProjectName { get; set; }
        public int TaskStatusId { get; set; }
        public string TaskStatus { get; set; }
        public DateTime TaskDueDate { get; set; }
        public string WorkspaceName { get; set; }
    }
}

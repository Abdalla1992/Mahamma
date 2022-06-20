using Mahamma.Base.Dto.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.TaskDDL
{
    public class TaskDDLQuery : IRequest<List<DropDownItem<int>>>
    {
        [DataMember]
        public int ProjectId { get; set; }
        public TaskDDLQuery(int projectId)
        {
            ProjectId = projectId;
        }
    }
}

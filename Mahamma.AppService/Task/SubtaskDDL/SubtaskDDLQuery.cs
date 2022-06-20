using Mahamma.Base.Dto.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.SubtaskDDL
{
    public class SubtaskDDLQuery : IRequest<List<DropDownItem<int>>>
    {
        [DataMember]
        public int TaskId { get; set; }
        public SubtaskDDLQuery(int taskId)
        {
            TaskId = taskId;
        }
    }
}

using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.TaskStatusTracker
{
    public class TaskStatusTrackerCommand : IRequest<int>
    {
        [DataMember]
        public int SkipCount { get; set; }
        public TaskStatusTrackerCommand(int skipCount)
        {
            SkipCount = skipCount;
        }
    }
}

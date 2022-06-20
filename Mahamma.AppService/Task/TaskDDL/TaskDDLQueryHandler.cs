using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Task.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.TaskDDL
{
    public class TaskDDLQueryHandler : IRequestHandler<TaskDDLQuery, List<DropDownItem<int>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        #endregion

        #region CTRS
        public TaskDDLQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        #endregion

        public async Task<List<DropDownItem<int>>> Handle(TaskDDLQuery request, CancellationToken cancellationToken)
        {
            return await _taskRepository.GetProjectTasksDDL(request.ProjectId);
        }
    }
}

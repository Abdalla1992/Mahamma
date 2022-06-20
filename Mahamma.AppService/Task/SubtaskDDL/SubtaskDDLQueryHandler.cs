using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Task.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.SubtaskDDL
{
    public class SubtaskDDLQueryHandler : IRequestHandler<SubtaskDDLQuery, List<DropDownItem<int>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        #endregion

        #region CTRS
        public SubtaskDDLQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        #endregion

        public async Task<List<DropDownItem<int>>> Handle(SubtaskDDLQuery request, CancellationToken cancellationToken)
        {
            return await _taskRepository.GetSubasksDDL(request.TaskId);
        }
    }
}

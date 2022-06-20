using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Task.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.ArchiveTaskList
{
    public class ArchiveTaskListCommandHandler : IRequestHandler<ArchiveTaskListCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;

        #endregion

        #region CTRS
        public ArchiveTaskListCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(ArchiveTaskListCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            await _taskRepository.ArchiveTaskList(request.TaskIdList);

            if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                //to Add MessageResourceReader with its layers
                response.Result.CommandMessage = "Data Archived Successfully";
            }
            else
            {
                response.Result.CommandMessage = "Failed to archive the task list. Try again shortly.";
            }
            return response;
        }
    }
}

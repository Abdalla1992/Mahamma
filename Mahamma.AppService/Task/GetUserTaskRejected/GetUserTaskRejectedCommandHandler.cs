using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.GetUserTaskRejected
{
    class GetUserTaskRejectedCommandHandler : IRequestHandler<GetUserTaskRejectedCommand, ValidateableResponse<ApiResponse<List<UserTaskAcceptedRejectedDto>>>>
    {
        private readonly ITaskReadRepository _taskReadRepository;

        public GetUserTaskRejectedCommandHandler(ITaskReadRepository taskReadRepository)
        {
            _taskReadRepository = taskReadRepository;
        }

        public async Task<ValidateableResponse<ApiResponse<List<UserTaskAcceptedRejectedDto>>>> Handle(GetUserTaskRejectedCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<UserTaskAcceptedRejectedDto>>> response = new(new ApiResponse<List<UserTaskAcceptedRejectedDto>>());
            List<UserTaskAcceptedRejectedDto> userTaskRejectes = await _taskReadRepository.GetUserTaskAcceptedRejectedStatus(request.UserId);
            if (userTaskRejectes.Count > 0)
            {
                response.Result.ResponseData = userTaskRejectes;
                response.Result.CommandMessage = $"Number Rejected Taskes is : {userTaskRejectes.Count()}";
            }
            else
            {
                response.Result.CommandMessage = "No Rejected Taskes For This User";
            }
            return response;
        }
    }
}

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
using Mahamma.Domain.Task.Enum;

namespace Mahamma.AppService.Task.GetUserTasks
{
    public class GetUserTasksCommandHandler : IRequestHandler<GetUserTasksCommand, ValidateableResponse<ApiResponse<UserProfileTaskHistoryDto>>>
    {
        private readonly ITaskReadRepository _taskReadRepository;

        public GetUserTasksCommandHandler(ITaskReadRepository taskReadRepository)
        {
            _taskReadRepository = taskReadRepository;
        }

        public async Task<ValidateableResponse<ApiResponse<UserProfileTaskHistoryDto>>> Handle(GetUserTasksCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<UserProfileTaskHistoryDto>> response = new(new ApiResponse<UserProfileTaskHistoryDto>());
            List<UserTaskDto> userTasks = await _taskReadRepository.GetUserTask(request.UserId);
            if (userTasks?.Count > 0)
            {
                UserProfileTaskHistoryDto userProfileTaskHistoryDto = new();
                userProfileTaskHistoryDto.UserTasks = userTasks;
                userProfileTaskHistoryDto.TotalTasks = userTasks.Count();

                userProfileTaskHistoryDto.CompletedTasks = userTasks.Where(t => t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedEarly.Id
                                                                        || t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedLate.Id
                                                                        || t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedOnTime.Id).Count();

                userProfileTaskHistoryDto.PendingTasks = userTasks.Where(t => t.TaskStatusId == Domain.Task.Enum.TaskStatus.New.Id
                                                                        || t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgress.Id
                                                                        || t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgressWithDelay.Id).Count();
                userProfileTaskHistoryDto.CompletedTaskPercentage = (userProfileTaskHistoryDto.CompletedTasks / userProfileTaskHistoryDto.TotalTasks) * 100;
                userProfileTaskHistoryDto.PendingTaskPercentage = (userProfileTaskHistoryDto.PendingTasks / userProfileTaskHistoryDto.TotalTasks) * 100;
                response.Result.ResponseData = userProfileTaskHistoryDto;
                double? rating = userTasks.Where(s => s.Rating.HasValue).Sum(t => t.Rating.Value);
                int ratedTaskCount = userTasks.Where(s => s.Rating.HasValue).Count();
                userProfileTaskHistoryDto.Rating = rating.HasValue && rating.Value > 0 ? rating.Value / ratedTaskCount : 0;
            }
            else
            {
                response.Result.ResponseData = new UserProfileTaskHistoryDto();
                response.Result.CommandMessage = "There are no tasks for this user";
            }
            return response;
        }
    }
}

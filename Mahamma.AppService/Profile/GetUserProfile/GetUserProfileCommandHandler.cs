using Mahamma.AppService.Profile.Dto;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.GetUserProfile.GetUserProfileCommand
{
    public class GetUserProfileCommandHandler : IRequestHandler<GetUserProfileCommand, ApiResponse<UserProfileDto>>
    {
        #region Props
        private readonly IHttpContextAccessor _httpContext;
        private readonly ITaskMemberRepository _taskMemberRepository;
        #endregion

        #region CTRS
        public GetUserProfileCommandHandler(IHttpContextAccessor httpContext, ITaskMemberRepository taskMemberRepository)
        {
            _httpContext = httpContext;
            _taskMemberRepository = taskMemberRepository;
        }
        #endregion

        public async Task<ApiResponse<UserProfileDto>> Handle(GetUserProfileCommand request, CancellationToken cancellationToken)
        {
            ApiResponse<UserProfileDto> response = new ApiResponse<UserProfileDto>();
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            IQueryable<TaskDto> taskList = await _taskMemberRepository.GetHistoryTasks(currentUser.Id);
            int completedTasks = taskList.Count(t => t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedEarly.Id || t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedOnTime.Id || t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedLate.Id);
            int pendingTasks = taskList.Count(t => t.TaskStatusId == Domain.Task.Enum.TaskStatus.New.Id);

            UserProfileDto result = new UserProfileDto
            {
                Email = currentUser.Email,
                ProfilePic = currentUser.ProfileImage,
                FullName = currentUser.FullName,
                JobTitle = currentUser.JobTitle,
                TotalTasks = taskList.Count(),
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                WorkingHours = currentUser.WorkingHours,
                TasksHistory = taskList.ToList(),
                Rating = 5,
            }; 
            return response;
        }
    }
}

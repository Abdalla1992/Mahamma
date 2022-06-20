using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Event;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Enum;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;

        #endregion

        #region ctor
        public UpdateTaskCommandHandler(ITaskRepository taskRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, ITaskMemberRepository taskMemberRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            INotificationService notificationService, IProjectRepository projectRepository,
             IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader, IAccountService accountService, IMediator mediator)
        {
            _taskRepository = taskRepository;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _httpContext = httpContext;
            _taskMemberRepository = taskMemberRepository;
            _notificationService = notificationService;
            _projectRepository = projectRepository;
            _messageResourceReader = messageResourceReader;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
            _mediator = mediator;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.Id);
            if (task != null)
            {
                var member = await _taskMemberRepository.GetMember(task.Id, currentUser.Id);
                if (member != null)
                {
                    task.UpdateTask(request.Name, request.Description, request.StartDate, request.DueDate, request.TaskPriorityId, request.ReviewRequest);

                    _taskRepository.UpdateTask(task);
                    await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.UpdateActivity, request.Id, currentUser.Id, cancellationToken);
                    

                    if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                    {
                        response.Result.ResponseData = true;
                        response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataUpdatedSuccessfully", currentUser.LanguageId);
                        #region SendNotification
                        Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(task.ProjectId);
                        List<TaskMember> taskMembers = await _taskMemberRepository.GetTaskMemberByTaskId(request.Id);
                        if (taskMembers?.Count > 0)
                        {
                            await TaskUpdatedEvent(task, currentUser);
                        }
                        #endregion
                    }
                    else
                    {
                        response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedTmodify", currentUser.LanguageId);
                    }
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId); ;
            }
            return response;
        }
        #region Methods
        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }

        private async Task<bool> TaskUpdatedEvent(Domain.Task.Entity.Task request, UserDto currentUser)
        {
            TaskUpdatedEvent taskUpdatedEvent = new(request, currentUser);
            await _mediator.Publish(taskUpdatedEvent);
            return true;
        }
        #endregion
    }
}

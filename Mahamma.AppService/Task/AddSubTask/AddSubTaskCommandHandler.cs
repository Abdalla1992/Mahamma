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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.AddSubTask
{
    public class AddSubTaskCommandHandler : IRequestHandler<AddSubTaskCommand, ValidateableResponse<ApiResponse<int>>>
    {
        #region Props
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly IHttpContextAccessor _httpContext;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;
        private readonly IProjectRepository _projectRepository;
        private readonly INotificationService _notificationService;

        #endregion

        #region ctor
        public AddSubTaskCommandHandler(ITaskRepository taskRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, IHttpContextAccessor httpContext, INotificationResourceReader notificationResourceReader,
            IAccountService accountService, IProjectRepository projectRepository, INotificationService notificationService, IMediator mediator)
        {
            _taskRepository = taskRepository;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _httpContext = httpContext;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
            _projectRepository = projectRepository;
            _notificationService = notificationService;
            _mediator = mediator;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<int>>> Handle(AddSubTaskCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<int>> response = new(new ApiResponse<int>());
            Domain.Task.Entity.Task task = new();

            List<TaskMember> taskMembers = request.UserIdList?.Select(uId =>
            {
                TaskMember taskMember = new();
                taskMember.CreateTaskMember(uId);
                return taskMember;
            }).ToList();
            TaskMember currentTaskMember = new();
            currentTaskMember.CreateTaskMember(currentUser.Id);
            taskMembers.Add(currentTaskMember);
            task.CreateTask(request.ProjectId, request.Name, request.Description, request.StartDate, request.DueDate,
                request.TaskPriorityId, request.ReviewRequest, request.ParentTaskId, currentUser.Id, taskMembers,
                0, request.IsCreatedFromMeeting, request.DependencyTaskId);

            _taskRepository.AddTask(task);
            if (task.ParentTaskId.HasValue)
            {
                //adding sub task is a task activity
                //but adding a task is project activity
                await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.SubTaskActivity, (int)request.ParentTaskId, currentUser.Id, cancellationToken);
            }

            if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                if (!task.ParentTaskId.HasValue)
                {
                    //but adding a task is project activity
                    await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.TaskActivity, (int)task.Id, currentUser.Id, cancellationToken);
                    await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.TaskActivity, (int)task.ProjectId, currentUser.Id, cancellationToken);
                }
                else
                {
                    double parentTaskProgressPercentage = await CalculateParentTaskProgressPercentage(task.ParentTaskId.Value);
                    Domain.Task.Entity.Task parentTask = await _taskRepository.GetTaskById(task.ParentTaskId.Value);
                    if (parentTask != null)
                    {
                        parentTask.ProgressPercentage = parentTaskProgressPercentage == 100 ? 90 : Math.Round(parentTaskProgressPercentage, 2);
                        _taskRepository.UpdateTask(parentTask);
                    }
                }
                Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.ProjectId);
                if (project != null)
                {
                    double projectProgressPercentage = await CalculateProjectProgressPercentage(task.ProjectId);
                    project.ProgressPercentage = Math.Round(projectProgressPercentage, 2);
                    _projectRepository.UpdateProject(project);
                    await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                }
                #region Send Notification
                if (request.UserIdList?.Count > 0 && project != null)
                {
                    await TaskAddedEvent(task, currentUser);
                }
                #endregion

                response.Result.ResponseData = task.Id;
                response.Result.CommandMessage = "Data Added Successfully ";
            }
            else
            {
                response.Result.CommandMessage = "Failed to add the new task. Try again shortly.";
            }
            return response;
        }
        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }
        private async Task<double> CalculateParentTaskProgressPercentage(int taskId)
        {
            double progressPercentage = default;
            var subtasks = await _taskRepository.GetSubtaskByTaskId(taskId);
            if (subtasks?.Count > default(int))
            {
                double progressPercentageSum = subtasks.Sum(s => s.ProgressPercentage);
                progressPercentage = progressPercentageSum / subtasks.Count;
            }
            return progressPercentage;
        }
        private async Task<double> CalculateProjectProgressPercentage(int projectId)
        {
            double progressPercentage = default;
            var tasks = await _taskRepository.GetTasksByProjectId(projectId);
            if (tasks?.Count > default(int))
            {
                double progressPercentageSum = tasks.Sum(s => s.ProgressPercentage);
                progressPercentage = progressPercentageSum / tasks.Count;
            }
            return progressPercentage;
        }

        private async Task<bool> TaskAddedEvent(Domain.Task.Entity.Task request, UserDto currentUser)
        {
            TaskAddedEvent taskAddedEvent = new(request, currentUser);
            await _mediator.Publish(taskAddedEvent);
            return true;
        }
    }
}

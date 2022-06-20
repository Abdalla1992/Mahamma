using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.UpdateTaskProgressPercentage
{
    public class UpdateTaskProgressPercentageCommandHandler : IRequestHandler<UpdateTaskProgressPercentageCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly IProjectRepository _projectRepository;
        private readonly ActivitesSettings _activitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        public UpdateTaskProgressPercentageCommandHandler(ITaskRepository taskRepository, ITaskActivityLogger taskActivityLogger,
            IProjectRepository projectRepository, ActivitesSettings activitesSettings,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader)
        {
            _taskRepository = taskRepository;
            _taskActivityLogger = taskActivityLogger;
            _projectRepository = projectRepository;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
        }

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateTaskProgressPercentageCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.TaskId);
            if (task != null)
            {
                task.ProgressPercentage = Math.Round(request.ProgressPercentage, 2);
                _taskRepository.UpdateTask(task);
                if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    if (task.ParentTaskId.HasValue)
                    {
                        double parentTaskProgressPercentage = await CalculateParentTaskProgressPercentage(task.ParentTaskId.Value);
                        Domain.Task.Entity.Task parentTask = await _taskRepository.GetTaskById(task.ParentTaskId.Value);
                        if (parentTask != null)
                        {
                            parentTask.ProgressPercentage = Math.Round(parentTaskProgressPercentage, 2);
                            _taskRepository.UpdateTask(parentTask);
                            await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                        }
                    }
                    Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(task.ProjectId);
                    if (project != null)
                    {
                        double projectProgressPercentage = await CalculateProjectProgressPercentage(task.ProjectId);
                        project.ProgressPercentage = Math.Round(projectProgressPercentage, 2);
                        _projectRepository.UpdateProject(project);
                        if (await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                        {
                            response.Result.ResponseData = true;
                            response.Result.CommandMessage = _messageResourceReader.GetKeyValue("UpdateProgressPercentageSuccessfully", currentUser.LanguageId);
                        }
                    }
                    await _taskActivityLogger.LogTaskActivity(_activitesSettings.UpdateTaskProgressPercentage, (int)task.Id,
                        currentUser.Id, cancellationToken);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedUpdateProgressPercentage", currentUser.LanguageId);
                }
            }
            return response;
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
    }
}

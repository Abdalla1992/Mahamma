using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Enum;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskStatus = Mahamma.Domain.Task.Enum.TaskStatus;

namespace Mahamma.AppService.Task.SubmitTask
{
    public class SubmitTaskCommandHandler : IRequestHandler<SubmitTaskCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;

        #endregion

        #region ctor
        public SubmitTaskCommandHandler(ITaskRepository taskRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, ITaskMemberRepository taskMemberRepository,
           INotificationService notificationService, IProjectRepository projectRepository,
             IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader, IAccountService accountService)
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

        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(SubmitTaskCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.TaskId);

            if (task != null)
            {
                if (await CanSubmitTask(task))
                {
                    var member = await _taskMemberRepository.GetMember(task.Id, currentUser?.Id ?? 1);
                    if (member != null)
                    {
                        task.TaskStatusId = SetTaskStatus(task);
                        task.ProgressPercentage = 100;
                        await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.TaskSubmittedActivity, request.TaskId, currentUser?.Id ?? 1, cancellationToken);

                        #region SendNotification

                        Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(task.ProjectId);
                        List<TaskMember> taskMembers = await _taskMemberRepository.GetTaskMemberByTaskId(request.TaskId);
                        if (taskMembers?.Count > 0)
                        {
                            List<NotificationDto> notificationListDto = new();
                            foreach (var item in taskMembers)
                            {
                                notificationListDto.Add(PrepareNotification(project.Id, project.WorkSpaceId, request.TaskId, currentUser.Id, item.UserId, task.Name));
                            }
                            await _notificationService.CreateNotificationList(notificationListDto);
                            //if()
                            //{
                            //    response.Result.ResponseData = true;
                            //    response.Result.CommandMessage = "notification Sent Successfully ";
                            //}
                            //else
                            //{
                            //    response.Result.ResponseData = false;
                            //    response.Result.CommandMessage = "notification Can`t Sent Now ";
                            //}
                        }
                        #endregion

                        if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                        {
                            await HandleTasksThatDependOnSubmittedTask(task.Id, cancellationToken);
                            if (task.ParentTaskId.HasValue && task.ParentTaskId.Value > 0)
                            {
                                double parentTaskProgressPercentage = await CalculateParentTaskProgressPercentage(task.ParentTaskId.Value);
                                Domain.Task.Entity.Task parentTask = await _taskRepository.GetTaskById(task.ParentTaskId.Value);
                                if (parentTask != null)
                                {
                                    parentTask.ProgressPercentage = parentTaskProgressPercentage == 100 ? 90 : parentTaskProgressPercentage;
                                    _taskRepository.UpdateTask(parentTask);
                                }
                            }
                            if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                            {
                                double projectProgressPercentage = await CalculateProjectProgressPercentage(task.ProjectId);
                                project.ProgressPercentage = projectProgressPercentage;
                                _projectRepository.UpdateProject(project);
                                await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                            }
                            response.Result.ResponseData = true;
                            response.Result.CommandMessage = "Task Submitted Successfully ";
                        }
                        else
                        {
                            response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToSubmitTask", currentUser.LanguageId);
                        }
                    }
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("SubmittedTaskHasnNotSubmittedSubtasks", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }

        #region Methods
        private int SetTaskStatus(Domain.Task.Entity.Task task)
        {
            return task.DueDate.Date < DateTime.Now.Date ? TaskStatus.CompletedLate.Id
                : task.DueDate.Date > DateTime.Now.Date ? TaskStatus.CompletedEarly.Id
                : task.DueDate.Date == DateTime.Now.Date ? TaskStatus.CompletedOnTime.Id : TaskStatus.New.Id;
        }

        private NotificationDto PrepareNotification(int projectId, int workSpaceId, int task, long senderId, long receiverUserId, string taskName)
        {
            UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, senderId);
            NotificationDto notificationDto = new NotificationDto
            {
                ProjectId = projectId,
                WorkSpaceId = workSpaceId,
                TaskId = task,
                SenderUserId = senderId,
                ReceiverUserId = receiverUserId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = NotificationType.SubmitTask.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.SubmitTask.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.SubmitTask.Id, Language.English.Id) + "{1} team", userDto.FullName, taskName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.SubmitTask.Id, Language.Arabic.Id) + "{1}", userDto.FullName, taskName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.SubmitTask.Id, Language.Arabic.Id)
            };
            return notificationDto;
        }
        private string GetNotificationBody(int NotificationTypeId, int languageId)
        {
            var name = NotificationType.From(NotificationTypeId);
            var message = _notificationResourceReader.GetKeyValue(name + "Body", languageId);
            return message;
        }
        private string GetNotificationTitle(int NotificationTypeId, int languageId)
        {
            var name = NotificationType.From(NotificationTypeId);
            var message = _notificationResourceReader.GetKeyValue(name + "Title", languageId);
            return message;
        }
        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }
        private async Task<bool> CanSubmitTask(Domain.Task.Entity.Task task)
        {
            if (task.ParentTaskId.HasValue && task.ParentTaskId.Value > 0)
            {
                return true;
            }
            else
            {
                var subtasks = await _taskRepository.GetSubtaskByTaskId(task.Id);
                if (subtasks?.Count > 0)
                {
                    return subtasks.Any(s => s.TaskStatusId != TaskStatus.CompletedEarly.Id && s.TaskStatusId != TaskStatus.CompletedLate.Id
                    && s.TaskStatusId != TaskStatus.CompletedOnTime.Id);
                }
                else
                {
                    return true;
                }
            }
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

        private async Task<bool> HandleTasksThatDependOnSubmittedTask(int taskId, CancellationToken cancellationToken)
        {
            bool result = false;
            List<Domain.Task.Entity.Task> tasks = await _taskRepository.GetTasksThatDependOn(taskId);
            if (tasks?.Count > 0)
            {
                foreach (var task in tasks)
                {
                    if (task.StartDate < DateTime.Now)
                    {
                        int hoursDiff = task.DueDate.Subtract(task.StartDate).Hours;
                        task.StartDate = DateTime.Now;
                        task.DueDate = task.StartDate.AddHours(hoursDiff);
                        _taskRepository.UpdateTask(task);
                    }
                }
                result = await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            return result;
        }
        #endregion
    }
}


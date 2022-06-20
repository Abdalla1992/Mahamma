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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.ArchiveTask
{
    public class ArchiveTaskCommandHandler : IRequestHandler<ArchiveTaskCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
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

        #region CTRS
        public ArchiveTaskCommandHandler(ITaskRepository taskRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, ITaskMemberRepository taskMemberRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, INotificationService notificationService, IProjectRepository projectRepository,
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

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(ArchiveTaskCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.Id);
            Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(task.ProjectId);


            if (task != null)
            {
                var member = await _taskMemberRepository.GetMember(task.Id, currentUser.Id);
                if (member != null)
                {
                    task.ArchiveTask();
                    await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.ArchiveActivity, request.Id, currentUser.Id, cancellationToken);

                    List<TaskMember> taskMembers = await _taskMemberRepository.GetTaskMemberByTaskId(request.Id);
                    if (taskMembers?.Count > 0)
                    {
                        #region SendNotification
                        List<NotificationDto> notificationListDto = new();
                        foreach (var item in taskMembers)
                        {
                            notificationListDto.Add(PrepareNotification(task.ProjectId, project.WorkSpaceId, task.Id, currentUser.Id, item.UserId, task.Name));
                        }
                        if (await _notificationService.CreateNotificationList(notificationListDto))
                        {
                            response.Result.ResponseData = true;
                            response.Result.CommandMessage = "notification Sent Successfully ";
                        }
                        else
                        {
                            response.Result.ResponseData = false;
                            response.Result.CommandMessage = "notification Can`t Sent Now ";
                        }
                    }
                    #endregion

                    if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                    {
                        response.Result.ResponseData = true;
                        response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataArchivedSuccessfully", currentUser.LanguageId);
                    }
                    else
                    {
                        response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToArchiveTheTask", currentUser.LanguageId);
                    }

                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToArchiveTheTask", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }
        private NotificationDto PrepareNotification(int projectId, int workSpaceId, int taskId, long senderId, long receiverUserId, string taskName)
        {
            UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, senderId);
            NotificationDto notificationDto = new NotificationDto
            {
                ProjectId = projectId,
                WorkSpaceId = workSpaceId,
                TaskId = taskId,
                SenderUserId = senderId,
                ReceiverUserId = receiverUserId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = NotificationType.ArchiveTask.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.ArchiveTask.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.ArchiveTask.Id, Language.English.Id) + "{1} team", userDto.FullName, taskName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.ArchiveTask.Id, Language.Arabic.Id) + "{1}", userDto.FullName, taskName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.ArchiveTask.Id, Language.Arabic.Id)
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
    }
}

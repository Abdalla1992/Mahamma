using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Enum;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProject
{
    class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, ValidateableResponse<ApiResponse<int>>>
    {
        #region Prop
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly ActivitesSettings _activitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationService _notificationService;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;

        #endregion

        #region Ctor
        public AddProjectCommandHandler(IProjectRepository projectRepository, IProjectActivityLogger projectActivityLogger,
            IWorkspaceRepository workspaceRepository, ActivitesSettings activitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader,
            INotificationService notificationService, INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _projectRepository = projectRepository;
            _projectActivityLogger = projectActivityLogger;
            _workspaceRepository = workspaceRepository;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            _notificationService = notificationService;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<int>>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<int>> response = new(new ApiResponse<int>());
            Domain.Project.Entity.Project project = new();
            List<ProjectMember> projectMembers = new();
            foreach (var userId in request.UserIdList)
            {
                ProjectMember projectMember = new ProjectMember();
                projectMember.CreateProjectMember(userId, project.Id);
                projectMembers.Add(projectMember);
            }
            ProjectMember currentProjectMember = new ProjectMember();
            currentProjectMember.CreateProjectMember(currentUser.Id, project.Id);
            projectMembers.Add(currentProjectMember);
            project.CreateProject(request.Name, request.Description, request.DueDate, request.WorkSpaceId, currentUser.Id, projectMembers,0, request.IsCreatedFromMeeting);
            _projectRepository.AddProject(project);

            if (await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                await _projectActivityLogger.LogProjectActivity(_activitesSettings.AddProject, project.Id, currentUser.Id, cancellationToken);


                Domain.Workspace.Entity.Workspace workspace = await _workspaceRepository.GetWorkspaceById(request.WorkSpaceId);
                #region Send Notification
                if (request.UserIdList?.Count > 0)
                {
                    List<NotificationDto> notificationListDto = new();
                    foreach (var uId in request.UserIdList)
                    {
                        notificationListDto.Add(PrepareNotification(project.Id, workspace.Id, currentUser.Id, uId, request.Name));
                    }
                    if (await _notificationService.CreateNotificationList(notificationListDto))
                    {
                        response.Result.ResponseData = project.Id;
                        response.Result.CommandMessage = "notification Sent Successfully ";
                    }
                    else
                    {
                        response.Result.ResponseData = project.Id;
                        response.Result.CommandMessage = "notification Can`t Sent Now ";
                    }
                }
                #endregion
                response.Result.ResponseData = project.Id;
                response.Result.CommandMessage = "Data Added Successfully";
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
            }
            return response;
        }

        private NotificationDto PrepareNotification(int projectId, int workSpaceId, long senderId, long receiverUserId, string projectName)
        {
            UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, senderId);
            NotificationDto notificationDto = new NotificationDto
            {
                ProjectId = projectId,
                WorkSpaceId = workSpaceId,
                SenderUserId = senderId,
                ReceiverUserId = receiverUserId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = NotificationType.AddProject.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AddProject.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.AddProject.Id, Language.English.Id) + "{1} team", userDto.FullName, projectName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.AddProject.Id, Language.Arabic.Id) + "{1}", userDto.FullName, projectName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.AddProject.Id, Language.Arabic.Id)
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

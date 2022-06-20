using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
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

namespace Mahamma.AppService.Project.UpdateProject
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly INotificationService _notificationService;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public UpdateProjectHandler(IProjectRepository projectRepository, IProjectActivityLogger projectActivityLogger,
            ActivitesSettings activitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader,
            IProjectMemberRepository projectMemberRepository, INotificationService notificationService,
            INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _projectRepository = projectRepository;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            _projectMemberRepository = projectMemberRepository;
            _notificationService = notificationService;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.Id);
            //Domain.Workspace.Entity.Workspace workspace = await _workspaceRepository.GetWorkspaceById(request.WorkSpaceId);

            if (project != null )
            {
                project.UpdateProject(request.Name, request.Description, request.DueDate, request.WorkSpaceId);
                _projectRepository.UpdateProject(project);
                await _projectActivityLogger.LogProjectActivity(_activitesSettings.UpdateProject, project.Id, currentUser.Id, cancellationToken);
               
                List<ProjectMember> projectMembers = await _projectMemberRepository.GetProjectMemberByProjectId(request.Id);
                if (projectMembers?.Count > 0)
                {
                    List<NotificationDto> notificationListDto = new();
                    foreach (var item in projectMembers)
                    {
                        notificationListDto.Add(PrepareNotification(request.Id, project.WorkSpaceId, currentUser.Id, item.UserId,request.Name));
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
             
                if (await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataUpdatedSuccessfully", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = $"{request.Name}" +_messageResourceReader.GetKeyValue("FailedTmodify", currentUser.LanguageId);
            }

            return response;
        }
        #region Methods
        private NotificationDto PrepareNotification(int projectId, int workSpaceId, long senderId, long receiverUserId,string projectName)
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
                NotificationTypeId = NotificationType.UpdateProject.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.UpdateProject.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.UpdateProject.Id, Language.English.Id) + "{1} team", userDto.FullName, projectName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.UpdateProject.Id, Language.Arabic.Id) + "{1}", userDto.FullName, projectName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.UpdateProject.Id, Language.Arabic.Id)
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
        #endregion
    }
}

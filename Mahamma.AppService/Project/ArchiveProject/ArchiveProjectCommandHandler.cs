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
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.ArchiveProject
{
    public class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, ValidateableResponse<ApiResponse<bool>>>
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
        public ArchiveProjectCommandHandler(IProjectRepository projectRepository, IProjectActivityLogger projectActivityLogger,
            ActivitesSettings activitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader,
            IProjectMemberRepository projectMemberRepository, INotificationService notificationService , INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _projectRepository = projectRepository;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _projectMemberRepository = projectMemberRepository;
            _messageResourceReader = messageResourceReader;
            _notificationService = notificationService;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;

        }
        #endregion

        #region Methods
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.Id);
            if (project != null)
            {
                project.ArchiveProject();
                await _projectActivityLogger.LogProjectActivity(_activitesSettings.ArchiveProject, request.Id, currentUser.Id, cancellationToken);


                List<ProjectMember> projectMembers = await _projectMemberRepository.GetProjectMemberByProjectId(request.Id);
                List<NotificationDto> notificationListDto = new();
                #region Send Notification
                if (projectMembers?.Count > 0)
                {
                    foreach (var item in projectMembers)
                    {
                        notificationListDto.Add(PrepareNotification(request.Id, project.WorkSpaceId, currentUser.Id, item.UserId, project.Name));
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

                if (await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataArchivedSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToArchiveTheProject", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }

        private NotificationDto PrepareNotification(int projectId, int workSpaceId, long senderId, long receiverUserId ,string projectName)
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
                NotificationTypeId = NotificationType.ArchiveProject.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.ArchiveProject.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.ArchiveProject.Id, Language.English.Id) + "{1} team", userDto.FullName, projectName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.ArchiveProject.Id, Language.Arabic.Id) + "{1}", userDto.FullName, projectName),
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

        #endregion
    }
}

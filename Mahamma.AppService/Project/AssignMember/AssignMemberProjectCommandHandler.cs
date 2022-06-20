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

namespace Mahamma.AppService.Project.AssignMember
{
    class AssignMemberProjectCommandHandler : IRequestHandler<AssignMemberProjectCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _activitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public AssignMemberProjectCommandHandler(IProjectMemberRepository projectMemberRepository, IProjectActivityLogger projectActivityLogger,
            ActivitesSettings activitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader,
            INotificationService notificationService, IProjectRepository projectRepository,INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectActivityLogger = projectActivityLogger;
            _activitesSettings = activitesSettings;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

            _notificationService = notificationService;
            _projectRepository = projectRepository;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AssignMemberProjectCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            List<ProjectMember> projectMembers = await _projectMemberRepository.GetProjectMemberByProjectId(request.ProjectId);
            await AssignMember(request, currentUser, response, projectMembers);
            await _projectActivityLogger.LogProjectActivity(_activitesSettings.AssignProjectMember, request.ProjectId, currentUser.Id, cancellationToken);

            if (await _projectMemberRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = "Member Added Successfully";
            }
            else
            {
                response.Result.CommandMessage = "Failed to add the new Member. Try again shortly.";
            }
            return response;
        }

        private async System.Threading.Tasks.Task AssignMember(AssignMemberProjectCommand request, UserDto currentUser, ValidateableResponse<ApiResponse<bool>> response, List<ProjectMember> projectMembers)
        {
            if (projectMembers?.Count > 0)
            {
                List<long> newMembers = request.UserIdList.Where(m => !projectMembers.Any(p => p.UserId == m)).ToList();
                if (newMembers?.Count > 0)
                {
                    ProjectMember projectMember = null;
                    foreach (var newMemberId in newMembers)
                    {
                        projectMember = new();
                        projectMember.CreateProjectMember(newMemberId, request.ProjectId);
                        _projectMemberRepository.AddProjectMember(projectMember);

                    }

                    // prepare to send notification
                    Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.ProjectId);
                    List<NotificationDto> notificationListDto = new();
                    foreach (var uId in newMembers)
                    {
                        notificationListDto.Add(PrepareNotification(request.ProjectId, project.WorkSpaceId, currentUser.Id, uId,project.Name));
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
                List<ProjectMember> deletedProjectMembers = projectMembers.Where(m => !request.UserIdList.Contains(m.UserId)).ToList();
                if (deletedProjectMembers?.Count > 0)
                {
                    foreach (var deletedMember in deletedProjectMembers)
                    {
                        deletedMember.DeleteProjectMember();
                    }
                    _projectMemberRepository.UpdateProjectMemberList(deletedProjectMembers);
                }
            }
            else
            {
                ProjectMember projectMember = null;
                foreach (var userId in request.UserIdList)
                {
                    projectMember = new();
                    projectMember.CreateProjectMember(userId, request.ProjectId);
                    _projectMemberRepository.AddProjectMember(projectMember);
                }

                // prepare to send notification
                Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.ProjectId);
                List<NotificationDto> notificationListDto = new();
                foreach (var uId in request.UserIdList)
                {
                    notificationListDto.Add(PrepareNotification(request.ProjectId, project.WorkSpaceId, currentUser.Id, uId,project.Name));
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
        }

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
                NotificationTypeId = NotificationType.AssignMemberToProject.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AssignMemberToProject.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.AssignMemberToProject.Id, Language.English.Id) + "{1} team", userDto.FullName, projectName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.AssignMemberToProject.Id, Language.Arabic.Id) + "{1}", userDto.FullName, projectName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.AssignMemberToProject.Id, Language.Arabic.Id)
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

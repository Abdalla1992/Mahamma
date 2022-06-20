using Mahamma.AppService.AtivityLogger.Project;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Project.Dto;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProjectComment
{
    public class AddProjectCommentCommandHandler : IRequestHandler<AddProjectCommentCommand, ValidateableResponse<ApiResponse<List<ProjectCommentDto>>>>
    {
        #region Props
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectCommentRepository _projectCommentRepository;
        private readonly IProjectActivityLogger _projectActivityLogger;
        private readonly ActivitesSettings _projectActivitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;

        #endregion


        #region ctor
        public AddProjectCommentCommandHandler(IProjectMemberRepository projectMemberRepository, IProjectCommentRepository projectCommentRepository,
            IProjectActivityLogger projectActivityLogger, ActivitesSettings projectActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            INotificationService notificationService, IProjectRepository projectRepository,
             IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectCommentRepository = projectCommentRepository;
            _projectActivityLogger = projectActivityLogger;
            _projectActivitesSettings = projectActivitesSettings;
            _httpContext = httpContext;
            _notificationService = notificationService;
            _projectRepository = projectRepository;
            _messageResourceReader = messageResourceReader;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<ProjectCommentDto>>>> Handle(AddProjectCommentCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<List<ProjectCommentDto>>> response = new(new ApiResponse<List<ProjectCommentDto>>());

            ProjectMemberDto projectMember = await _projectMemberRepository.GetMemberByIdForMakeComment(request.ProjectId, currentUser.Id);
            if (projectMember != null)
            {
                ProjectComment projectComment = new();
                projectComment.CreateComment(request.Comment, request.ProjectId, projectMember.Id, request.ParentCommentId, request.ImageUrl, currentUser.Id);

                _projectCommentRepository.AddComment(projectComment);
                _projectActivityLogger.LogProjectActivity(_projectActivitesSettings.CommentActivity, request.ProjectId, projectMember.Id, cancellationToken);
                Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.ProjectId);

                #region Send Notification
                List<NotificationDto> notificationListDto = new();
                List<ProjectMember> projectMsmbers = await _projectMemberRepository.GetProjectMemberByProjectIdExceptCurrentUser(request.ProjectId, projectMember.UserId);
                if (projectMsmbers?.Count > 0)
                {
                    foreach (var member in projectMsmbers)
                    {
                        if (request.MentionedUserList.Contains(member.UserId))
                        {
                            notificationListDto.Add(PrepareMentionNotification(project.Id, project.WorkSpaceId, currentUser.Id, member.UserId, project.Name));
                        }
                        else
                        {
                            notificationListDto.Add(PrepareNotification(project.Id, project.WorkSpaceId, currentUser.Id, member.UserId, project.Name));
                        }
                    }

                    if (await _notificationService.CreateNotificationList(notificationListDto))
                    {
                        List<ProjectCommentDto> projectComments = await _projectCommentRepository.GetProjectComment(request.ProjectId);
                        if (projectComments?.Count > 0)
                        {
                            projectComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                            foreach (var comment in projectComments)
                            {
                                comment.Replies.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                            }
                        }
                        response.Result.ResponseData = projectComments;
                        response.Result.CommandMessage = "notification Sent Successfully ";
                    }
                    else
                    {
                        List<ProjectCommentDto> projectComments = await _projectCommentRepository.GetProjectComment(request.ProjectId);
                        if (projectComments?.Count > 0)
                        {
                            projectComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                            foreach (var comment in projectComments)
                            {
                                comment.Replies.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                            }
                        }
                        response.Result.ResponseData = projectComments;
                        response.Result.CommandMessage = "notification Can`t Sent Now ";
                    }
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToComment", currentUser.LanguageId);
                }
                #endregion

                if (await _projectCommentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    List<ProjectCommentDto> projectComments = await _projectCommentRepository.GetProjectComment(request.ProjectId);
                    if (projectComments?.Count > 0)
                    {
                        projectComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                        foreach (var comment in projectComments)
                        {
                            comment.Replies.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                        }
                    }
                    response.Result.ResponseData = projectComments;
                    response.Result.CommandMessage = "Data Addeded Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "FailedToAddTheNewComment";
                }

            }
            return response;
        }
        #region Priveate Methods
        private NotificationDto PrepareMentionNotification(int projectId, int workSpaceId, long senderId, long receiverUserId, string projectName)
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
                NotificationTypeId = NotificationType.MentionComment.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.MentionComment.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.MentionComment.Id, Language.English.Id) + "{1} team", userDto.FullName, projectName),
                NotificationBodyArabic = string.Format("{0} " + GetNotificationBody(NotificationType.MentionComment.Id, Language.Arabic.Id) + "{1}", userDto.FullName, projectName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.MentionComment.Id, Language.Arabic.Id)
            };
            return notificationDto;
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
                NotificationTypeId = NotificationType.AddComment.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AddComment.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.AddComment.Id, Language.English.Id) + "{1} team", userDto.FullName, projectName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.AddComment.Id, Language.Arabic.Id) + "{1}", userDto.FullName, projectName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.AddComment.Id, Language.Arabic.Id)
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
        private List<MemberDto> SetMembers(params long[] userIdList)
        {
            var result = new List<MemberDto>();
            foreach (var userId in userIdList)
            {
                UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, userId);
                if (userDto != null)
                {
                    result.Add(new MemberDto
                    {
                        UserId = userDto.Id,
                        FullName = userDto.FullName,
                        ProfileImage = userDto.ProfileImage
                    });
                }
            }
            return result;
        }
        #endregion


    }
}

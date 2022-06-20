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

namespace Mahamma.AppService.Project.LikeProjectComment
{
    public class LikeProjectCommentCommandHandler : IRequestHandler<LikeProjectCommentCommand, ValidateableResponse<ApiResponse<List<ProjectCommentDto>>>>
    {
        #region Props
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectCommentRepository _projectCommentRepository;
        private readonly IProjectLikeCommentRepository _projectLikeCommentRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;
        #endregion



        #region CTRS
        public LikeProjectCommentCommandHandler(IProjectMemberRepository projectMemberRepository, IProjectCommentRepository projectCommentRepository,
            IProjectLikeCommentRepository projectLikeCommentRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
             INotificationService notificationService, IProjectRepository projectRepository,
              IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectCommentRepository = projectCommentRepository;
            _projectLikeCommentRepository = projectLikeCommentRepository;
            _httpContext = httpContext;
            _notificationService = notificationService;
            _projectRepository = projectRepository;
            _messageResourceReader = messageResourceReader;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;

        }
        #endregion


        public async Task<ValidateableResponse<ApiResponse<List<ProjectCommentDto>>>> Handle(LikeProjectCommentCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<ProjectCommentDto>>> response = new(new ApiResponse<List<ProjectCommentDto>>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var member = await _projectMemberRepository.GetMemberByIdForMakeComment(request.ProjectId, currentUser.Id);

            if (member != null && await _projectCommentRepository.ValidToLikeComment(request.ProjectId, request.CommentId))
            {
                await _projectLikeCommentRepository.LikeComment(member.Id, request.CommentId);
                Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.ProjectId);
                ProjectComment projectComment = await _projectCommentRepository.GetEntityById(request.CommentId);
                ProjectMember projectmember = await _projectMemberRepository.GetProjectCommentId(projectComment.ProjectMemberId);

                #region Send Notification
                List<NotificationDto> notificationListDto = new();
                notificationListDto.Add(PrepareNotification(project.Id, project.WorkSpaceId, currentUser.Id, projectmember.UserId, project.Name));
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
                    List<ProjectCommentDto> taskComments = await _projectCommentRepository.GetProjectComment(request.ProjectId);
                    if (taskComments?.Count > 0)
                    {
                        taskComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                        foreach (var comment in taskComments)
                        {
                            comment.Replies.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                        }
                    }
                    response.Result.ResponseData = taskComments;
                    response.Result.CommandMessage = "notification Can`t Sent Now ";
                }
                #endregion

                if (await _projectLikeCommentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    List<ProjectCommentDto> projectComments = await _projectCommentRepository.GetProjectComment(request.ProjectId);
                    if (projectComments?.Count > 0)
                    {
                        //projectComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());

                        foreach (var comment in projectComments)
                        {
                            comment.Member = SetMembers(comment.UserId ?? 0).FirstOrDefault();
                            comment.IsLikedByCurrentUser = _projectRepository.CheckCommentLikedByCurrentUser(comment.Id, currentUser.Id);

                            //comment.Replies.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                            foreach (var reply in comment.Replies)
                            {
                                reply.Member = SetMembers(reply.UserId ?? 0).FirstOrDefault();
                                reply.IsLikedByCurrentUser = _projectRepository.CheckCommentLikedByCurrentUser(reply.Id, currentUser.Id);
                            }
                        }
                    }
                    response.Result.ResponseData = projectComments;
                    response.Result.CommandMessage = "Liked Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed To Like";
                }
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
                NotificationTypeId = NotificationType.LikeComment.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.LikeComment.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.LikeComment.Id, Language.English.Id) + "{1} team", userDto.FullName, projectName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.LikeComment.Id, Language.Arabic.Id) + "{1}", userDto.FullName, projectName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.LikeComment.Id, Language.Arabic.Id)
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

    }
}

using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Dto;
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

namespace Mahamma.AppService.Task.LikeComment
{
    public class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand, ValidateableResponse<ApiResponse<List<TaskCommentDto>>>>
    {
        #region Props
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly ITaskCommentRepository _taskCommentRepository;
        private readonly ILikeCommentRepository _likeCommentRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;

        #endregion

        #region CTRS
        public LikeCommentCommandHandler(ITaskMemberRepository taskMemberRepository, ITaskCommentRepository taskCommentRepository,
            ILikeCommentRepository likeCommentRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
             INotificationService notificationService, IProjectRepository projectRepository, ITaskRepository taskRepository,
              IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _taskMemberRepository = taskMemberRepository;
            _taskCommentRepository = taskCommentRepository;
            _likeCommentRepository = likeCommentRepository;
            _httpContext = httpContext;
            _notificationService = notificationService;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _messageResourceReader = messageResourceReader;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;

        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<TaskCommentDto>>>> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<TaskCommentDto>>> response = new(new ApiResponse<List<TaskCommentDto>>());

            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            var member = await _taskMemberRepository.GetMember(request.TaskId, currentUser.Id);

            if (member != null && await _taskCommentRepository.ValidToLikeComment(request.TaskId, request.CommentId))
            {
                await _likeCommentRepository.LikeComment(member.Id, request.CommentId);
                Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.TaskId);
                Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(task.ProjectId);

                TaskComment taskComment = await _taskCommentRepository.GetEntityById(request.CommentId);
                TaskMember taskmember =await _taskMemberRepository.GetById(taskComment.TaskMemberId);

                #region Send Notification

                List<NotificationDto> notificationListDto = new();           
                notificationListDto.Add(PrepareNotification(project.Id, project.WorkSpaceId,request.TaskId , currentUser.Id, taskmember.UserId,task.Name));
                if (await _notificationService.CreateNotificationList(notificationListDto))
                {
                    List<TaskCommentDto> taskComments = await _taskCommentRepository.GetTaskComment(request.TaskId);
                    if (taskComments?.Count > 0)
                    {
                        taskComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                        foreach (var comment in taskComments)
                        {
                            comment.Replies.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                        }
                    }
                    response.Result.ResponseData = taskComments;
                    response.Result.CommandMessage = "notification Sent Successfully ";
                }
                else
                {
                    List<TaskCommentDto> taskComments = await _taskCommentRepository.GetTaskComment(request.TaskId);
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

                if (await _likeCommentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    List<TaskCommentDto> taskComments = await _taskCommentRepository.GetTaskComment(request.TaskId);
                    if (taskComments?.Count > 0)
                    {
                        //taskComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                        foreach (var comment in taskComments)
                        {
                            comment.Member = SetMembers(comment.UserId ?? 0).FirstOrDefault();
                            comment.IsLikedByCurrentUser = _taskRepository.CheckCommentLikedByCurrentUser(comment.Id, currentUser.Id);

                            //comment.Replies.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
                            foreach (var reply in comment.Replies)
                            {
                                reply.Member = SetMembers(reply.UserId ?? 0).FirstOrDefault();
                                reply.IsLikedByCurrentUser = _taskRepository.CheckCommentLikedByCurrentUser(reply.Id, currentUser.Id);
                            }
                        }
                    }
                    response.Result.ResponseData = taskComments;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("LikedSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToLike", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToLike", currentUser.LanguageId);
            }
            return response;
        }
        #region Methods
        private NotificationDto PrepareNotification(int projectId, int workSpaceId,int taskId, long senderId, long receiverUserId,string taskName)
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
                NotificationTypeId = NotificationType.LikeComment.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.LikeComment.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.LikeComment.Id, Language.English.Id) + "{1} team", userDto.FullName, taskName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.LikeComment.Id, Language.Arabic.Id) + "{1}", userDto.FullName, taskName),
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
        #endregion

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

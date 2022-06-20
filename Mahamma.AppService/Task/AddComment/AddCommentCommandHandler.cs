using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
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
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.AddComment
{
    public class AddCommentCommand : IRequest<ValidateableResponse<ApiResponse<List<TaskCommentDto>>>>
    {
        #region Props
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public List<long> MentionedUserList { get; set; }
        [DataMember]
        public int? ParentCommentId { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        #endregion
    }
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, ValidateableResponse<ApiResponse<List<TaskCommentDto>>>>
    {
        #region Props
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly ITaskCommentRepository _taskCommentRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;

        #endregion

        #region ctor
        public AddCommentCommandHandler(ITaskMemberRepository taskMemberRepository, ITaskCommentRepository taskCommentRepository,
            ITaskActivityLogger taskActivityLogger, ActivitesSettings taskActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            INotificationService notificationService, IProjectRepository projectRepository, ITaskRepository taskRepository,
             IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _taskMemberRepository = taskMemberRepository;
            _taskCommentRepository = taskCommentRepository;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _httpContext = httpContext;
            _notificationService = notificationService;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _messageResourceReader = messageResourceReader;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;

        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<List<TaskCommentDto>>>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<List<TaskCommentDto>>> response = new(new ApiResponse<List<TaskCommentDto>>());

            TaskMemberDto taskMember = await _taskMemberRepository.GetMember(request.TaskId, currentUser.Id);
            if (taskMember != null)
            {
                TaskComment taskComment = new();
                taskComment.CreateComment(request.Comment, request.TaskId, taskMember.Id, request.ParentCommentId, request.ImageUrl, currentUser.Id);

                _taskCommentRepository.AddComment(taskComment);
                _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.CommentActivity, request.TaskId, taskMember.Id, cancellationToken);
                Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.TaskId);
                Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(task.ProjectId);

                #region Send Notification
                List<NotificationDto> notificationListDto = new();
                List<TaskMember> taskMsmbers = await _taskMemberRepository.GetTaskMemberByTaskIdExceptCurrentUser(request.TaskId, taskMember.UserId);
                if (taskMsmbers?.Count > 0)
                {
                    foreach (var member in taskMsmbers)
                    {
                        if (request.MentionedUserList.Contains(member.UserId))
                        {
                            notificationListDto.Add(PrepareMentionNotification(project.Id, project.WorkSpaceId, request.TaskId, currentUser.Id, member.UserId, task.Name));
                        }
                        else
                        {
                            notificationListDto.Add(PrepareNotification(project.Id, project.WorkSpaceId, request.TaskId, currentUser.Id, member.UserId, task.Name));
                        }
                    }

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
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToComment", currentUser.LanguageId);
                }
                #endregion

                if (await _taskCommentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
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
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataAddededSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewComment", currentUser.LanguageId);
                }
                
             }
            return response;
        }
        #region Methods
        private NotificationDto PrepareMentionNotification(int projectId, int workSpaceId, int taskId, long senderId, long receiverUserId, string taskName)
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
                NotificationTypeId = NotificationType.MentionComment.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.MentionComment.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.MentionComment.Id, Language.English.Id) + "{1} team", userDto.FullName, taskName),
                NotificationBodyArabic = string.Format("{0} " + GetNotificationBody(NotificationType.MentionComment.Id, Language.Arabic.Id) + "{1}", userDto.FullName, taskName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.MentionComment.Id, Language.Arabic.Id)
            };
            return notificationDto;
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
                NotificationTypeId = NotificationType.AddComment.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AddComment.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.AddComment.Id, Language.English.Id) + "{1} team", userDto.FullName, taskName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.AddComment.Id, Language.Arabic.Id) + "{1}", userDto.FullName, taskName),
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

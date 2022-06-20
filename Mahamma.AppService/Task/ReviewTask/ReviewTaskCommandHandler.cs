using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
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
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.ReviewTask
{
    public class ReviewTaskCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public List<TaskMemberDto> TaskMembers { get; set; }
        [DataMember]
        public bool Accpeted { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion
    }
    public class ReviewTaskCommandHandler : IRequestHandler<ReviewTaskCommand, ValidateableResponse<ApiResponse<bool>>>
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
        private readonly ITaskCommentRepository _taskCommentRepository;

        #endregion

        #region ctor
        public ReviewTaskCommandHandler(ITaskRepository taskRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, ITaskMemberRepository taskMemberRepository,
           INotificationService notificationService, IProjectRepository projectRepository,
             IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader,
             IAccountService accountService, ITaskCommentRepository taskCommentRepository)
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
            _taskCommentRepository = taskCommentRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(ReviewTaskCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.TaskId, "TaskMembers,Project");

            if (task != null)
            {
                //foreach (var member in task.TaskMembers)
                //{
                //    member.Rating = request.TaskMembers?.FirstOrDefault(tm => tm.UserId == member.UserId).Rating;
                //}
                if (request.Accpeted)// Accepted == true
                {
                    foreach (var member in task.TaskMembers.Where(m => m.UserId != task.CreatorUserId))
                    {
                        //member.Rating = request.TaskMembers?.FirstOrDefault(tm => tm.UserId == member.UserId).Rating;

                        if ((member.TaskAcceptedRejectedStatus == Domain.Task.Enum.TaskAcceptedRejectedStatus.NotReviewed.Id) || (member.TaskAcceptedRejectedStatus == Domain.Task.Enum.TaskAcceptedRejectedStatus.Accepted.Id))
                        {
                            member.TaskAcceptedRejectedStatus = Domain.Task.Enum.TaskAcceptedRejectedStatus.Accepted.Id;
                        }
                        else if (member.TaskAcceptedRejectedStatus == Domain.Task.Enum.TaskAcceptedRejectedStatus.Rejected.Id)
                        {
                            member.TaskAcceptedRejectedStatus = Domain.Task.Enum.TaskAcceptedRejectedStatus.AcceptedAfterRejected.Id;
                        }
                    }
                    await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.TaskAcceptedActivity, request.TaskId, currentUser?.Id ?? 1, cancellationToken);
                }
                else // Acceptef == false;
                {
                    foreach (var taskMember in task.TaskMembers.Where(m => m.UserId != task.CreatorUserId))
                    {
                        //taskMember.Rating = request.TaskMembers?.FirstOrDefault(tm => tm.UserId == taskMember.UserId).Rating;
                        if (taskMember.TaskAcceptedRejectedStatus == Domain.Task.Enum.TaskAcceptedRejectedStatus.NotReviewed.Id || taskMember.TaskAcceptedRejectedStatus == Domain.Task.Enum.TaskAcceptedRejectedStatus.Accepted.Id || taskMember.TaskAcceptedRejectedStatus == Domain.Task.Enum.TaskAcceptedRejectedStatus.Rejected.Id)
                        {
                            taskMember.TaskAcceptedRejectedStatus = Domain.Task.Enum.TaskAcceptedRejectedStatus.Rejected.Id;
                        }
                    }
                    await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.TaskRejectedActivity + $" Reason : \"{request.Description}\" ", request.TaskId, currentUser?.Id ?? 1, cancellationToken);
                    TaskComment taskComment = new();
                    int taskMemberId = task.TaskMembers.FirstOrDefault(t => t.UserId == currentUser.Id).Id;
                    taskComment.CreateComment(request.Description, task.Id, taskMemberId, null, string.Empty, currentUser.Id);
                    _taskCommentRepository.AddComment(taskComment);
                }
                var userIdList = task.TaskMembers.Select(m => m.UserId);
                List<NotificationDto> notificationListDto = new();
                foreach (var uid in userIdList)
                {
                    notificationListDto.Add(PrepareNotification(request.Accpeted, task.ProjectId, task.Project.WorkSpaceId, task.Id, currentUser, uid, task.Name));
                }
                if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    await _notificationService.CreateNotificationList(notificationListDto);
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("SuccessToReviewTask", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToReviewTask", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }

        #region Methods
        private NotificationDto PrepareNotification(bool isAccepted, int projectId, int workSpaceId, int task, UserDto sender, long receiverUserId, string taskName)
        {
            NotificationDto notificationDto = new NotificationDto
            {
                ProjectId = projectId,
                WorkSpaceId = workSpaceId,
                TaskId = task,
                SenderUserId = sender.Id,
                ReceiverUserId = receiverUserId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = isAccepted ? NotificationType.AcceptedTask.Id : NotificationType.RejectTask.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AcceptedTask.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(isAccepted ? NotificationType.AcceptedTask.Id : NotificationType.RejectTask.Id, Language.English.Id) + "{1} team", sender.FullName, taskName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(isAccepted ? NotificationType.AcceptedTask.Id : NotificationType.RejectTask.Id, Language.Arabic.Id) + "{1}", sender.FullName, taskName),
                NotificationTitleArabic = GetNotificationTitle(isAccepted ? NotificationType.AcceptedTask.Id : NotificationType.RejectTask.Id, Language.Arabic.Id)
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


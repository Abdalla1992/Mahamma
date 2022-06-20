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

namespace Mahamma.AppService.Task.AssignMember
{
    public class AssignMemberCommandHandler : IRequestHandler<AssignMemberCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;
        #endregion

        #region ctor
        public AssignMemberCommandHandler( ITaskMemberRepository taskMemberRepository,
            ITaskActivityLogger taskActivityLogger, ActivitesSettings taskActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            INotificationService notificationService, IProjectRepository projectRepository, ITaskRepository taskRepository,
            IMessageResourceReader messageResourceReader,INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _taskMemberRepository = taskMemberRepository;
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
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AssignMemberCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            List<long> taskMembersUserIdList = await _taskMemberRepository.GetMembersUserIdList(request.TaskId);

            #region Add New Members
            List<long> newMembersUserIdList = FilterExistingMembers(request.UserIdList, taskMembersUserIdList);
            List<TaskMember> newMembers = NewMembersList(request.TaskId, newMembersUserIdList);
            _taskMemberRepository.AssingMemberListToTask(newMembers);

            Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.TaskId);
            #region Send Notification
            Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(task.ProjectId);
            List<NotificationDto> notificationListDto = new();
            if (newMembers?.Count > 0)
            {
                foreach (var uId in newMembers)
                {
                    notificationListDto.Add(PrepareNotification(task.ProjectId, project.WorkSpaceId, request.TaskId, currentUser.Id, uId.UserId, task.Name));
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

            #endregion
            #region Removed Members
            List<long> removedMembersUserIdList = FilterRemovedMembers(request.UserIdList, taskMembersUserIdList);
            await RemoveMembersListFromTask(request.TaskId, removedMembersUserIdList);
            #endregion
            await _taskActivityLogger.LogTaskActivity(_taskActivitesSettings.AssignTaskMember, request.TaskId, currentUser.Id, cancellationToken);

            if (await _taskMemberRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage =  _messageResourceReader.GetKeyValue("MemberAddedSuccessfully",currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddNewMember", currentUser.LanguageId);
            }
            return response;
        }

        private async System.Threading.Tasks.Task RemoveMembersListFromTask(int taskId, List<long> removedMembersUserIdList)
        {
            List<TaskMember> members = await _taskMemberRepository.GetMembersList(taskId, removedMembersUserIdList);
            members.ForEach(m => m.RemoveUserFromTask());
        }

        private List<TaskMember> NewMembersList(int taskId, List<long> userIdList)
        {
            List<TaskMember> result = new();
            foreach (var userId in userIdList)
            {
                TaskMember member = new();
                member.CreateTaskMember(userId, taskId);
                result.Add(member);
            }
            return result;
        }

        private List<long> FilterRemovedMembers(List<long> userIdList, List<long> taskMembersUserIdList)
        {
            return taskMembersUserIdList.Where(u => !userIdList.Contains(u)).ToList();
        }

        private List<long> FilterExistingMembers(List<long> userIdList, List<long> taskMembersUserIdList)
        {
            return userIdList.Where(u => !taskMembersUserIdList.Contains(u)).ToList();
        }

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
                NotificationTypeId = NotificationType.AssignMemberToTask.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AssignMemberToTask.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.AssignMemberToTask.Id, Language.English.Id) + "{1} team", userDto.FullName, taskName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.AssignMemberToTask.Id, Language.Arabic.Id) + "{1}", userDto.FullName, taskName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.AssignMemberToTask.Id, Language.Arabic.Id)
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

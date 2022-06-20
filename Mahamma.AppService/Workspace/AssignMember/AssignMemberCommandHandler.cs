using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Workspace.Entity;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.AssignMember
{
    public class AssignMemberCommandHandler : IRequestHandler<AssignWorkSpaceMemberCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region MyRegion
        private readonly IWorkspaceMemberRepository _workSpaceMemberRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public AssignMemberCommandHandler(IWorkspaceMemberRepository workSpaceMemberRepository ,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext ,INotificationService notificationService,
            IWorkspaceRepository workspaceRepository, INotificationResourceReader notificationResourceReader, IAccountService accountService)
        {
            _workSpaceMemberRepository = workSpaceMemberRepository;
            _httpContext = httpContext;
            _notificationService = notificationService;
            _workspaceRepository = workspaceRepository;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AssignWorkSpaceMemberCommand request, CancellationToken cancellationToken)
        {

            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            List<WorkspaceMember> workSpaceMembers = await _workSpaceMemberRepository.GetWorkSpaceMemberById(request.WorkSpaceId);
            Domain.Workspace.Entity.Workspace workspace =await _workspaceRepository.GetWorkspaceById(request.WorkSpaceId);
            if (workSpaceMembers?.Count > 0)
            {
                List<long> newMembers = request.UserIds.Where(m => !workSpaceMembers.Any(p => p.UserId == m)).ToList();
                if (newMembers?.Count > 0)
                {
                    WorkspaceMember workspaceMember = null;
                    foreach (var newMemberId in newMembers)
                    {
                        workspaceMember = new();
                        workspaceMember.CreateWorkspaceMember(newMemberId, request.WorkSpaceId);
                        _workSpaceMemberRepository.AddWorkSpaceMember(workspaceMember);

                    }

                    // prepare to send notification
                    List<NotificationDto> notificationListDto = new();
                    foreach (var uId in newMembers)
                    {
                        notificationListDto.Add(PrepareNotification( request.WorkSpaceId, currentUser.Id, uId,workspace.Name));
                    }
                    if (await _notificationService.CreateNotificationList(notificationListDto))
                    {
                        response.Result.ResponseData = true;
                        response.Result.CommandMessage = "notification Sent Successfully ";
                    }
                    else
                    {
                        response.Result.ResponseData = false;
                        response.Result.CommandMessage = "Anotification Can`t Sent Now ";
                    }

                }
                List<WorkspaceMember> deletedWorkSpaceMembers = workSpaceMembers.Where(m => !request.UserIds.Contains(m.UserId)).ToList();
                if (deletedWorkSpaceMembers?.Count > 0)
                {
                    foreach (var deletedMember in deletedWorkSpaceMembers)
                    {
                        deletedMember.DeleteWorkspaceMember();
                    }
                    _workSpaceMemberRepository.UpdateWorkSpaceMemberList(deletedWorkSpaceMembers);
                }
            }
            else
            {
                WorkspaceMember workspaceMember = null;
                foreach (var userId in request.UserIds)
                {
                    workspaceMember = new();
                    workspaceMember.CreateWorkspaceMember(userId, request.WorkSpaceId);
                    _workSpaceMemberRepository.AddWorkSpaceMember(workspaceMember);
                }

                // prepare to send notification
                List<NotificationDto> notificationListDto = new();
                foreach (var uId in request.UserIds)
                {
                    notificationListDto.Add(PrepareNotification(request.WorkSpaceId, currentUser.Id, uId,workspace.Name));
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
            if (await _workSpaceMemberRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
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

        #region  Methods
        private NotificationDto PrepareNotification(int workSpaceId, long senderId, long receiverUserId,string workspaceName)
        {
            UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, senderId);
            NotificationDto notificationDto = new NotificationDto
            {
                WorkSpaceId = workSpaceId,
                SenderUserId = senderId,
                ReceiverUserId = receiverUserId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = NotificationType.AssignMemberToWorkspace.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AssignMemberToWorkspace.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.AssignMemberToWorkspace.Id, Language.English.Id) + "{1} team", userDto.FullName, workspaceName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.AssignMemberToWorkspace.Id, Language.Arabic.Id) + "{1}", userDto.FullName, workspaceName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.AssignMemberToWorkspace.Id, Language.Arabic.Id)
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

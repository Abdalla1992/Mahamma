using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Workspace.Entity;
using Mahamma.Domain.Workspace.Enum;
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

namespace Mahamma.AppService.Workspace.AddWorkspace
{
    public class AddWorkspaceCommandHandler : IRequestHandler<AddWorkspaceCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IAccountService _accountService;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly INotificationService _notificationService;
        #endregion

        #region ctor
        public AddWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IAccountService accountService, IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader,
            INotificationService notificationService)
        {
            _workspaceRepository = workspaceRepository;
            _httpContext = httpContext;
            _accountService = accountService;
            _messageResourceReader = messageResourceReader;
            _notificationResourceReader = notificationResourceReader;
            _notificationService = notificationService;
        }
        #endregion

        #region Methods
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddWorkspaceCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Workspace.Entity.Workspace workspace = new();

            #region Create WorkspaceMember
            List<WorkspaceMember> workSpaceMembers = new();
            foreach (var userId in request.UserIdList)
            {
                WorkspaceMember workSpaceMember = new();
                workSpaceMember.CreateWorkspaceMember(userId, workspace.Id);
                workSpaceMembers.Add(workSpaceMember);
            }
            #endregion

            #region Create CompaneWorkspace
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            WorkspaceMember currentWorkSpaceMember = new();
            currentWorkSpaceMember.CreateWorkspaceMember(currentUser.Id, workspace.Id);
            workSpaceMembers.Add(currentWorkSpaceMember);
            #endregion

            bool isFirstWorkspaceInCompany = await _workspaceRepository.CheckWorkspaceIsFirstInCompany(currentUser.CompanyId);
            if (isFirstWorkspaceInCompany)
            {
                //update user profile status
                await _accountService.UpdateUserProfileStatus(new BaseRequestDto() { AuthToken = GetAccessToken() }, UserProfileStatus.FirstWorkspaceCreated.Id);
            }
            workspace.CreateWorkspace(request.Name, request.ImageUrl, request.Color, currentUser.CompanyId, currentUser.Id, workSpaceMembers);
            _workspaceRepository.AddWorkspace(workspace);

            if (await _workspaceRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                #region SendNotification

                if (request.UserIdList?.Count >0)
                {
                    List<NotificationDto> notificationListDto = new();
                    foreach (var uId in request.UserIdList)
                    {
                        notificationListDto.Add(PrepareNotification(workspace.Id, currentUser.Id, uId, request.Name));
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

                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataAddededSuccessfully", currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
            }
            return response;
        }

        private NotificationDto PrepareNotification(int workSpaceId, long senderId, long receiverUserId, string workspaceName)
        {
            UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, senderId);
            NotificationDto notificationDto = new NotificationDto
            {
                WorkSpaceId = workSpaceId,
                SenderUserId = senderId,
                ReceiverUserId = receiverUserId,
                NotificationSendingStatusId = NotificationSendingStatus.New.Id,
                NotificationSendingTypeId = NotificationSendingType.All.Id,
                NotificationTypeId = NotificationType.AddWorkspace.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.AddWorkspace.Id, Language.English.Id),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.AddWorkspace.Id, Language.English.Id) + "{1} team", userDto.FullName, workspaceName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.AddWorkspace.Id, Language.Arabic.Id) + "{1}", userDto.FullName, workspaceName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.AddWorkspace.Id, Language.Arabic.Id)
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

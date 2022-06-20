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

namespace Mahamma.AppService.Workspace.UpdateWorkspace
{
    public class UpdateWorkspaceCommandHandler : IRequestHandler<UpdateWorkspaceCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IWorkspaceMemberRepository _workSpaceMemberRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;

        private readonly INotificationService _notificationService;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;


        #endregion

        #region ctor
        public UpdateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository, IWorkspaceMemberRepository workSpaceMemberRepository, IMessageResourceReader messageResourceReader,
                 Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, INotificationService notificationService,
            INotificationResourceReader notificationResourceReader, IAccountService accountService)

        {
            _workspaceRepository = workspaceRepository;
            _workSpaceMemberRepository = workSpaceMemberRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;

            _notificationService = notificationService;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            Domain.Workspace.Entity.Workspace workspace = await _workspaceRepository.GetWorkspaceById(request.Id);
            if (workspace != null)
            {

                //await AssignWorkSpaceMember(request);
                List<WorkspaceMember> workSpaceMembers = await _workSpaceMemberRepository.GetWorkSpaceMemberById(request.Id);
                if (workSpaceMembers?.Count > 0)
                {
                    List<long> newMember = request.UserIdList?.Where(m => !workSpaceMembers.Any(x => x.UserId == m)).ToList();
                    if (newMember?.Count > 0)
                    {
                        WorkspaceMember workSpaceMember = null;
                        foreach (var newMemberId in newMember)
                        {
                            workSpaceMember = new();
                            workSpaceMember.CreateWorkspaceMember(newMemberId, request.Id);
                            _workSpaceMemberRepository.AddWorkSpaceMember(workSpaceMember);
                        }

                        #region Send Notification
                        List<NotificationDto> notificationListDto = new();
                        foreach (var uId in newMember)
                        {
                            notificationListDto.Add(PrepareNotification(request.Id, currentUser.Id, uId, request.Name));
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
                        #endregion
                    }
                    List<WorkspaceMember> deletedWorkSpaceMembers = workSpaceMembers.Where(m => !request.UserIdList.Contains(m.UserId)).ToList();
                    if (deletedWorkSpaceMembers?.Count > 0)
                    {
                        foreach (var deleteMember in deletedWorkSpaceMembers)
                        {
                            deleteMember.DeleteWorkspaceMember();
                        }
                        _workSpaceMemberRepository.UpdateWorkSpaceMemberList(deletedWorkSpaceMembers);
                    }
                }
                else
                {
                    foreach (var userId in request.UserIdList)
                    {
                        WorkspaceMember workSpaceMember = new();
                        workSpaceMember.CreateWorkspaceMember(userId, request.Id);
                        _workSpaceMemberRepository.AddWorkSpaceMember(workSpaceMember);
                    }

                    #region Send Notification
                    List<NotificationDto> notificationListDto = new();
                    foreach (var uId in request.UserIdList)
                    {
                        notificationListDto.Add(PrepareNotification(request.Id, currentUser.Id, uId, request.Name));
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
                    #endregion
                }
                workspace.UpdateWorkspace(request.Name, request.ImageUrl, request.Color);

                _workspaceRepository.UpdateWorkspace(workspace);

                if (await _workspaceRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    //to Add MessageResourceReader with its layers
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataUpdatedSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedTmodify", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }

        //private async System.Threading.Tasks.Task AssignWorkSpaceMember(UpdateWorkspaceCommand request)
        //{
        //    List<WorkspaceMember> workSpaceMembers = await _workSpaceMemberRepository.GetWorkSpaceMemberById(request.Id);
        //    if (workSpaceMembers?.Count > 0)
        //    {
        //        List<long> newMember = request.UserIdList?.Where(m => !workSpaceMembers.Any(x => x.UserId == m)).ToList();
        //        if (newMember?.Count > 0)
        //        {
        //            WorkspaceMember workSpaceMember = null;
        //            foreach (var newMemberId in newMember)
        //            {
        //                workSpaceMember = new();
        //                workSpaceMember.CreateWorkspaceMember(newMemberId, request.Id);
        //                _workSpaceMemberRepository.AddWorkSpaceMember(workSpaceMember);
        //            }

        //            #region Send Notification
        //            // prepare to send notification
        //            List<NotificationDto> notificationListDto = new();
        //            foreach (var uId in newMember)
        //            {
        //                notificationListDto.Add(PrepareNotification(request.Id, currentUser.Id, uId, project.Name));
        //            }
        //            if (await _notificationService.CreateNotificationList(notificationListDto))
        //            {
        //                response.Result.ResponseData = true;
        //                response.Result.CommandMessage = "notification Sent Successfully ";
        //            }
        //            else
        //            {
        //                response.Result.ResponseData = false;
        //                response.Result.CommandMessage = "notification Can`t Sent Now ";
        //            }
        //            #endregion
        //        }
        //        List<WorkspaceMember> deletedWorkSpaceMembers = workSpaceMembers.Where(m => !request.UserIdList.Contains(m.UserId)).ToList();
        //        if (deletedWorkSpaceMembers?.Count > 0)
        //        {
        //            foreach (var deleteMember in deletedWorkSpaceMembers)
        //            {
        //                deleteMember.DeleteWorkspaceMember();
        //            }
        //            _workSpaceMemberRepository.UpdateWorkSpaceMemberList(deletedWorkSpaceMembers);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var userId in request.UserIdList)
        //        {
        //            WorkspaceMember workSpaceMember = new();
        //            workSpaceMember.CreateWorkspaceMember(userId, request.Id);
        //            _workSpaceMemberRepository.AddWorkSpaceMember(workSpaceMember);
        //        }
        //        #region Send Notification
        //        // prepare to send notification
        //        Domain.Project.Entity.Project project = await _projectRepository.GetProjectById(request.ProjectId);
        //        List<NotificationDto> notificationListDto = new();
        //        foreach (var uId in request.UserIdList)
        //        {
        //            notificationListDto.Add(PrepareNotification(request.ProjectId, project.WorkSpaceId, currentUser.Id, uId, project.Name));
        //        }
        //        if (await _notificationService.CreateNotificationList(notificationListDto))
        //        {
        //            response.Result.ResponseData = true;
        //            response.Result.CommandMessage = "notification Sent Successfully ";
        //        }
        //        else
        //        {
        //            response.Result.ResponseData = false;
        //            response.Result.CommandMessage = "notification Can`t Sent Now ";
        //        }
        //        #endregion

        //    }
        //}

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
                NotificationTypeId = NotificationType.UpdateWorkspace.Id,
                IsRead = false,
                NotificationTitleEnglish = GetNotificationTitle(NotificationType.UpdateWorkspace.Id, 1),
                NotificationBodyEnglish = string.Format("{0}" + GetNotificationBody(NotificationType.UpdateWorkspace.Id, 1) + "{1} team", userDto.FullName, workspaceName),
                NotificationBodyArabic = string.Format("{0} قام" + GetNotificationBody(NotificationType.UpdateWorkspace.Id, 2) + "{1}", userDto.FullName, workspaceName),
                NotificationTitleArabic = GetNotificationTitle(NotificationType.UpdateWorkspace.Id, 2)
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

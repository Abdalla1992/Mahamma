using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.GetTask
{
    public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, ValidateableResponse<ApiResponse<TaskDto>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IProjectRepository _projectRepository;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly AppSetting _appSetting;
        private readonly IMeetingRepository _meetingRepository;

        #endregion

        #region CTRS
        public GetTaskQueryHandler(ITaskRepository taskRepository, IAccountService accountService,
            IHttpContextAccessor httpContext, IProjectAttachmentRepository projectAttachmentRepository,
            IMessageResourceReader messageResourceReader,
            IProjectRepository projectRepository, AppSetting appSetting,IMeetingRepository meetingRepository)
        {
            _taskRepository = taskRepository;
            _messageResourceReader = messageResourceReader;
            _projectRepository = projectRepository;
            _accountService = accountService;
            _httpContext = httpContext;
            _projectAttachmentRepository = projectAttachmentRepository;
            _appSetting = appSetting;
            _meetingRepository = meetingRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<TaskDto>>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<TaskDto>> response = new(new ApiResponse<TaskDto>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            TaskDto taskDto = await _taskRepository.GetTaskData(request.Id, currentUser.Id, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.CompanyId, request.RequestedFromMeeting);
            taskDto.UpComingMeetingDate = await _meetingRepository.GetTaskUpComingMeetingDate(request.Id);
            taskDto.TaskMinuteOfMeetings = await _meetingRepository.GetMinuteOfMeetingByTaskId(request.Id);
            
            if (taskDto.ParentTaskId.HasValue)
            {
                var parentTask = await _taskRepository.GetTaskById((int)taskDto.ParentTaskId);
                taskDto.ParentTaskName = parentTask.Name;
            }
            taskDto.Members = SetMembers(taskDto.TaskMembers.Select(m => m.UserId).ToArray());
            MapTaskMembers(taskDto.TaskMembers, taskDto.Members);

            //taskDto.TaskComments.ForEach(c => c.Member = SetMembers(c.UserId ?? 0).FirstOrDefault());
            foreach (var comment in taskDto.TaskComments)
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


            PageList<ProjectAttachmentDto> pageList = await _projectAttachmentRepository.GetTaskLatestFileList(request.Id, 3);
            taskDto.TaskAttachments = pageList.DataList;
            taskDto.FilesCount = pageList.TotalCount;
            if (taskDto != null)
            {
                var project = await _projectRepository.GetById(taskDto.ProjectId, currentUser.Id, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.CompanyId);
                taskDto.WorkspaceId = project != null ? project.WorkSpaceId : default(int);
                response.Result.CommandMessage = "Process completed successfully.";
                response.Result.ResponseData = taskDto;
            }
            else
            {
                response.Result.CommandMessage = "No data found.";
            }
            return response;
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

        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }

        private void MapTaskMembers(List<TaskMemberDto> taskMembers, List<MemberDto> members)
        {
            foreach (var member in members)
            {
                taskMembers.FirstOrDefault(m => m.UserId == member.UserId).FullName = member.FullName;
            }
        }
    }
}

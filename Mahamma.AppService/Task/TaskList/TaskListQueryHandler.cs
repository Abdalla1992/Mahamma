using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Repository;
using Mahamma.Domain.Workspace.Dto;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.TaskList
{
    public class TaskListQueryHandler : IRequestHandler<SearchTaskDto, PageList<TaskDto>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        private readonly IAccountService _accountService;
        private readonly ITaskMemberRepository _taskMemberRepository;
        #endregion

        #region CTRS
        public TaskListQueryHandler(ITaskRepository taskRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, AppSetting appSetting,
            ITaskMemberRepository taskMemberRepository, IAccountService accountService)
        {
            _taskRepository = taskRepository;
            _httpContext = httpContext;
            _appSetting = appSetting;
            _taskMemberRepository = taskMemberRepository;
            _accountService = accountService;
        }
        #endregion

        public async Task<PageList<TaskDto>> Handle(SearchTaskDto request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            PageList<TaskDto> taskList = await _taskRepository.GetTaskList(request, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.Id, currentUser.CompanyId);
            foreach (var task in taskList.DataList)
            {
                //#region Get subtasks
                //task.SubTasks = await _taskRepository.GetSubtaskByTaskId(task.Id);
                //#endregion

                #region Get task members
                task.Members = new List<Domain.MemberSearch.Dto.MemberDto>();
                List<long> membersIds = await _taskMemberRepository.GetMembersUserIdList(task.Id);
                if (membersIds?.Count > 0)
                {
                    foreach (var memberId in membersIds)
                    {
                        UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, memberId);
                        if (userDto != null)
                        {
                            task.Members.Add(new Domain.MemberSearch.Dto.MemberDto
                            {
                                UserId = userDto.Id,
                                FullName = userDto.FullName,
                                ProfileImage = userDto.ProfileImage
                            });
                        }
                    }
                } 
                #endregion
            }
            return taskList;
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

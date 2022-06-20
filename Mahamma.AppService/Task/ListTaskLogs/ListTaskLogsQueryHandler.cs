using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
using Mahamma.Domain.TaskActivity.Dto;
using Mahamma.Domain.TaskActivity.Repository;
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

namespace Mahamma.AppService.Task.ListTaskLogs
{
    public class ListTaskLogsQueryHandler : IRequestHandler<ListTaskLogsQuery, ValidateableResponse<ApiResponse<List<TaskActivityDto>>>>
    {
        #region Props
        private readonly ITaskActivityRepository _taskActivityRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAccountService _accountService;
        private readonly ITaskMemberRepository _taskMemberRepository;
        #endregion

        #region CTRS
        public ListTaskLogsQueryHandler(ITaskActivityRepository taskActivityRepository, IHttpContextAccessor httpContext,
            IAccountService accountService, ITaskMemberRepository taskMemberRepository)
        {
            _taskActivityRepository = taskActivityRepository;
            _httpContext = httpContext;
            _accountService = accountService;
            _taskMemberRepository = taskMemberRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<TaskActivityDto>>>> Handle(ListTaskLogsQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<TaskActivityDto>>> response = new(new ApiResponse<List<TaskActivityDto>>());
            var taskActivityList = await _taskActivityRepository.GetLogs(request.TaskId);
            if (taskActivityList?.Count > 0)
            {
                foreach (var item in taskActivityList)
                {
                    if (!item.UserId.HasValue)
                    {
                        TaskMember taskMember = await _taskMemberRepository.GetById(item.TaskMemberId);
                        if (taskMember != null)
                        {
                            item.Member = SetMembers(taskMember.UserId);
                        }
                    }
                    else
                    {
                        item.Member = SetMembers(item.UserId.Value);
                    }
                }
            }

            if (taskActivityList != null && taskActivityList.Count > 0)
            {
                response.Result.CommandMessage = $"{taskActivityList.Count} file found";
                response.Result.ResponseData = taskActivityList;
            }
            else
            {
                response.Result.CommandMessage = $"No date found.";
            }
            return response;
        }

        private MemberDto SetMembers(long userId)
        {
            var result = new MemberDto();
            // foreach (var userId in userIdList)
            // {
            UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, userId);
            if (userDto != null)
            {
                result = new MemberDto
                {
                    UserId = userDto.Id,
                    FullName = userDto.FullName,
                    ProfileImage = userDto.ProfileImage
                };
            }
            // }
            return result;
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

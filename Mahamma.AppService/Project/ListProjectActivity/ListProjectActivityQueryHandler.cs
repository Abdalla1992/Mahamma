using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectActivity.Dto;
using Mahamma.Domain.ProjectActivity.Repository;
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

namespace Mahamma.AppService.Project.ListProjectActivity
{
    public class ListProjectActivityQueryHandler : IRequestHandler<ListProjectActivityQuery, ValidateableResponse<ApiResponse<List<ProjectActivityDto>>>>
    {
        #region Prop
        private readonly IProjectActivityRepository _projectActivityRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region Ctor
        public ListProjectActivityQueryHandler(IProjectActivityRepository projectActivityRepository,
            IProjectMemberRepository projectMemberRepository, IAccountService accountService, IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader)
        {
            _projectActivityRepository = projectActivityRepository;
            _projectMemberRepository = projectMemberRepository;
            _accountService = accountService;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;

        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<List<ProjectActivityDto>>>> Handle(ListProjectActivityQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<ProjectActivityDto>>> response = new(new ApiResponse<List<ProjectActivityDto>>());
            List<ProjectActivityDto> projectActivityList = await _projectActivityRepository.GetProjectActivityList(request.ProjectId);
            if (projectActivityList?.Count > 0)
            {
                foreach (var item in projectActivityList)
                {
                    ProjectMember projectMember = await _projectMemberRepository.GetProjectMemberById(item.ProjectMemberId);
                    if (projectMember != null)
                    {
                        UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, projectMember.UserId);
                        item.MemberProfileImage = userDto?.ProfileImage;
                    }
                }
                response.Result.CommandMessage = $"{projectActivityList.Count} file found";
                response.Result.ResponseData = projectActivityList;
            }
            else
            {
                response.Result.CommandMessage = "No Activities Found";
            }
            return response;
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

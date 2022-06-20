using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Workspace.Dto;
using Mahamma.Domain.Workspace.Entity;
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

namespace Mahamma.AppService.Workspace.GetWorkspace
{
    public class GetWorkspaceQueryHandler : IRequestHandler<GetWorkspaceQuery, ValidateableResponse<ApiResponse<WorkspaceUserDto>>>
    {
        #region Props
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IWorkspaceMemberRepository _workSpaceMemberRepository;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        #endregion

        #region CTRS
        public GetWorkspaceQueryHandler(IWorkspaceRepository workspaceRepository, IWorkspaceMemberRepository workSpaceMemberRepository,
            IAccountService accountService, IHttpContextAccessor httpContext, AppSetting appSetting)
        {
            _workspaceRepository = workspaceRepository;
            _workSpaceMemberRepository = workSpaceMemberRepository;
            _accountService = accountService;
            _httpContext = httpContext;
            _appSetting = appSetting;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<WorkspaceUserDto>>> Handle(GetWorkspaceQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<WorkspaceUserDto>> response = new(new ApiResponse<WorkspaceUserDto>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            WorkspaceDto workspaceDto = await _workspaceRepository.GetById(request.Id, currentUser.Id, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.CompanyId);

            if (workspaceDto != null)
            {
                WorkspaceUserDto workspaceUserDto = new WorkspaceUserDto
                {
                    Id = workspaceDto.Id,
                    Name = workspaceDto.Name,
                    CompanyId = workspaceDto.CompanyId,
                    ImageUrl = workspaceDto.ImageUrl,
                    Color = workspaceDto.Color,
                    CreatorUserId = workspaceDto.CreatorUserId,
                    Members = new List<Domain.MemberSearch.Dto.MemberDto>(),
                    UserIdList = new List<long>()
                };
                List<WorkspaceMember> workspaceMembers = await _workSpaceMemberRepository.GetWorkSpaceMemberById(workspaceDto.Id);
                if (workspaceMembers?.Count > 0)
                {
                    foreach (var member in workspaceMembers)
                    {
                        UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, member.UserId);
                        if (userDto != null)
                        {
                            workspaceUserDto.Members.Add(new Domain.MemberSearch.Dto.MemberDto
                            {
                                UserId = userDto.Id,
                                FullName = userDto.FullName,
                                ProfileImage = userDto.ProfileImage
                            });
                            workspaceUserDto.UserIdList.Add(userDto.Id);
                        }
                    }
                }
                response.Result.CommandMessage = $"Process completed successfully.";
                response.Result.ResponseData = workspaceUserDto;
            }
            else
            {
                response.Result.CommandMessage = "No data found.";
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

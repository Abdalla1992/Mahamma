using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mahamma.Domain.Project.Dto;
using System.Threading;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.AppService.Settings;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Domain.Project.Entity;
using Microsoft.Extensions.Primitives;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Identity.ApiClient.Dto.Base;

namespace Mahamma.AppService.Project.ListProject
{
    class ListProjectQueryHandler : IRequestHandler<SearchProjectDto, PageList<ProjectUserDto>>
    {
        #region Prop
        private readonly IProjectRepository _projectRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public ListProjectQueryHandler(IProjectRepository projectRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, AppSetting appSetting,
            IProjectMemberRepository projectMemberRepository, IAccountService accountService)
        {
            _projectRepository = projectRepository;
            _httpContext = httpContext;
            _appSetting = appSetting;
            _projectMemberRepository = projectMemberRepository;
            _accountService = accountService;
        }
        #endregion

        public async Task<PageList<ProjectUserDto>> Handle(SearchProjectDto request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            PageList<ProjectUserDto> projectUsers = await _projectRepository.GetProjectData(request, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.Id, currentUser.CompanyId);
            if (projectUsers != null && projectUsers.DataList?.Count > 0)
            {
                foreach (ProjectUserDto project in projectUsers.DataList)
                {
                    project.Members = new List<Domain.MemberSearch.Dto.MemberDto>();
                    project.UserIdList = new List<long>();
                    List<ProjectMember> projectMembers = await _projectMemberRepository.GetProjectMemberByProjectId(project.Id);
                    if (projectMembers?.Count > 0)
                    {
                        foreach (var member in projectMembers)
                        {
                            UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, member.UserId);
                            if (userDto != null)
                            {
                                project.Members.Add(new Domain.MemberSearch.Dto.MemberDto
                                {
                                    UserId = userDto.Id,
                                    FullName = userDto.FullName,
                                    ProfileImage = userDto.ProfileImage,
                                    //Rating = member.Rating
                                });
                                project.UserIdList.Add(userDto.Id);
                            }
                        }
                    }
                }
            }
            return projectUsers;
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

using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.GetProjectTaskSubtaskNames
{
    public class GetUserProjectsQuery : IRequest<ApiResponse<List<ProjectDto>>>
    { }
    public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, ApiResponse<List<ProjectDto>>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        public GetUserProjectsQueryHandler(IProjectRepository projectRepository, IHttpContextAccessor httpContext, AppSetting appSetting)
        {
            _projectRepository = projectRepository;
            _httpContext = httpContext;
            _appSetting = appSetting;
        }
        public async Task<ApiResponse<List<ProjectDto>>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ApiResponse<List<ProjectDto>> response = new ApiResponse<List<ProjectDto>>();
            List<ProjectDto> projectList = null;
            if (currentUser.RoleName == _appSetting.SuperAdminRole)
            {
                projectList = await _projectRepository.GetProjectListByCompanyId(currentUser.CompanyId);
            }
            else
            {
                projectList = await _projectRepository.GetProjectListByUser(currentUser.Id);
            }
            if (projectList != null)
            {
                response.ResponseData = projectList;
            }
            else
            {
                response.CommandMessage = "There is no project in system for this project id";
            }
            return response;
        }
    }
}

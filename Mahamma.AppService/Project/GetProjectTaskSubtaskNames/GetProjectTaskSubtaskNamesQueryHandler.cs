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
    public class GetProjectTaskSubtaskNamesQueryHandler : IRequestHandler<GetProjectTaskSubtaskNamesQuery, ValidateableResponse<ApiResponse<ProjectTaskSubtaskNamesDto>>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly AppSetting _appSetting;
        private readonly IHttpContextAccessor _httpContext;
        public GetProjectTaskSubtaskNamesQueryHandler(IProjectRepository projectRepository, ITaskRepository taskRepository,
            AppSetting appSetting,IHttpContextAccessor httpContext)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _appSetting = appSetting;
            _httpContext = httpContext;
        }
        public async Task<ValidateableResponse<ApiResponse<ProjectTaskSubtaskNamesDto>>> Handle(GetProjectTaskSubtaskNamesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<ProjectTaskSubtaskNamesDto>> response = new(new ApiResponse<ProjectTaskSubtaskNamesDto>());
            ProjectDto project = await _projectRepository.GetById(request.ProjectId, currentUser.Id, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.CompanyId);
            if (project != null)
            {
                ProjectTaskSubtaskNamesDto projectTaskSubtaskNamesDto = new();
                projectTaskSubtaskNamesDto.ProjectName = project.Name;
                if (request.TaskId.HasValue)
                {
                    TaskDto task = await _taskRepository.GetById(request.TaskId.Value);
                    if (task != null)
                    {
                        projectTaskSubtaskNamesDto.TaskName = task.Name;
                        if (task.ParentTaskId.HasValue)
                        {
                            projectTaskSubtaskNamesDto.SubtaskName = task?.Name;
                            TaskDto parentTask = await _taskRepository.GetById(task.ParentTaskId.Value);
                            projectTaskSubtaskNamesDto.TaskName = parentTask.Name;
                        }                        
                    }
                }
                response.Result.ResponseData = projectTaskSubtaskNamesDto;
            }
            else
            {
                response.Result.CommandMessage = "There is no project in system for this project id";
            }
            return response;
        }
    }
}

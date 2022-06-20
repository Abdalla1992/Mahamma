using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Workspace.Dto;
using Mahamma.Domain.Workspace.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Workspace.ListWorkspace
{
    public class ListWorkspaceQueryHandler : IRequestHandler<SearchWorkspaceDto, PageList<WorkspaceDto>>
    {
        #region Props
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        #endregion

        #region CTRS
        public ListWorkspaceQueryHandler(IWorkspaceRepository workspaceRepository,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, AppSetting appSetting)
        {
            _workspaceRepository = workspaceRepository;
            _httpContext = httpContext;
            _appSetting = appSetting;
        }
        #endregion

        public async Task<PageList<WorkspaceDto>> Handle(SearchWorkspaceDto request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            request.Filter.CompanyId = currentUser.CompanyId;
            return await _workspaceRepository.GetWorkspaceData(request, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.Id, currentUser.CompanyId);
        }
    }
}

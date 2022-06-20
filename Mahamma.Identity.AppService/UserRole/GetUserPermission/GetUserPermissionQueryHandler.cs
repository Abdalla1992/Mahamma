using Mahamma.Base.Domain.Enum;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.UserRole.GetUserPermission
{
    public class GetUserPermissionQueryHandler : IRequestHandler<GetUserPermissionQuery, ValidateableResponse<ApiResponse<List<int>>>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePagePermissionRepository _rolePagePermissionRepository;
        private readonly IPagePermissionRepository _pagePermissionRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        #region ctor
        public GetUserPermissionQueryHandler(IUserRoleRepository userRoleRepository, IRolePagePermissionRepository rolePagePermissionRepository,
            IPagePermissionRepository pagePermissionRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _userRoleRepository = userRoleRepository;
            _rolePagePermissionRepository = rolePagePermissionRepository;
            _pagePermissionRepository = pagePermissionRepository;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<List<int>>>> Handle(GetUserPermissionQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<int>>> response = new(new ApiResponse<List<int>>());
            long userId = (long)_httpContext.HttpContext.Items["UserId"];
            long roleId = await _userRoleRepository.GetRoleIdByUserId(userId);
            if (roleId > default(long))
            {
                List<int> pagePermissionIds = await _rolePagePermissionRepository.GetIdsByRoleId(roleId);
                if (pagePermissionIds?.Count > default(int))
                {
                    List<int> permissions = await _pagePermissionRepository.GetPermissionsIds(pagePermissionIds);
                    if (permissions.Count > default(int))
                    {
                        response.Result.ResponseData = permissions;
                        response.Result.CommandMessage = "Data processed successfully";
                    }
                    else
                    {
                        response.Result.CommandMessage = $"User doesn't have permissions for this page {((PageEnum)request.PageId).ToString()}";
                    }
                }
                else
                {
                    response.Result.CommandMessage = $"User doesn't have permissions for this page {((PageEnum)request.PageId).ToString()}";
                }
            }
            else
            {
                response.Result.CommandMessage = $"User doesn't have permissions for this page {((PageEnum)request.PageId).ToString()}";
            }
            return response;
        }
    }
}

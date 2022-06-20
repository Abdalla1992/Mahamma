using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Dto;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.GetAllPagePermission
{
    public class GetAllPagePermissionCommand : IRequest<ApiResponse<List<PagePermissionDto>>>
    {
    }
    class GetAllPagePermissionCommandHandler : IRequestHandler<GetAllPagePermissionCommand, ApiResponse<List<PagePermissionDto>>>
    {
        #region Prop
        private readonly IPagePermissionRepository _pagePermissionRepository;
        private readonly IPageLocalizationRepository _pageLocalizationRepository;
        private readonly IPermissionLocalizationRepository _permissionLocalizationRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;
        #endregion

        #region Ctor
        public GetAllPagePermissionCommandHandler(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IPagePermissionRepository pagePermissionRepository, IPageLocalizationRepository pageLocalizationRepository,
            IPermissionLocalizationRepository permissionLocalizationRepository, IUserRepository userRepository)
        {
            _httpContext = httpContext;
            _pagePermissionRepository = pagePermissionRepository;
            _pageLocalizationRepository = pageLocalizationRepository;
            _permissionLocalizationRepository = permissionLocalizationRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region Method
        public async Task<ApiResponse<List<PagePermissionDto>>> Handle(GetAllPagePermissionCommand request, CancellationToken cancellationToken)
        {
            ApiResponse<List<PagePermissionDto>> response = new ApiResponse<List<PagePermissionDto>>();
            long currentUserId = (long)_httpContext.HttpContext.Items["UserId"];
            UserDto currentUser = await _userRepository.GetById(currentUserId);
            List<PagePermissionDto> pagePermissionList = await _pagePermissionRepository.GetAllPagePermissions();
            //IQueryable<Domain.Role.Entity.Role> roleList = _roleManager.Roles;
            if (pagePermissionList.Count() > 0)
            {
                PageLocalization pageLocalization = null;
                PermissionLocalization permissionLocalization = null;
                foreach (var pagePermission in pagePermissionList)
                {
                    pageLocalization = await _pageLocalizationRepository.GetByPageIdAndLanguageId(pagePermission.PageId, currentUser.LanguageId);
                    permissionLocalization = await _permissionLocalizationRepository.GetByPermissionIdAndLanguageId(pagePermission.PermissionId, currentUser.LanguageId);
                    pagePermission.PageName = pageLocalization?.DisplayName;
                    pagePermission.PermissionName = permissionLocalization?.DisplayName;
                }
                response.ResponseData = pagePermissionList;
                response.CommandMessage = "Role Saved Succefuly";
            }
            else
            {
                response.CommandMessage = "Failed to add the new Role. Try again shortly.";
            }
            return response;
        }
        #endregion
    }
}

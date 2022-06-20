using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.SetCompanyBasicRoles
{
    class SetCompanyBasicRolesCommandHandler : IRequestHandler<SetCompanyBasicRolesCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IRoleRepository _roleRepository;
        private readonly IPagePermissionRepository _pagePermissionRepository;
        private readonly IRolePagePermissionRepository _rolePagePermissionRepository;
        private readonly AppSetting _appSetting;

        #endregion

        #region Ctor
        public SetCompanyBasicRolesCommandHandler(AppSetting appSetting, IRoleRepository roleRepository,
            IPagePermissionRepository pagePermissionRepository, IRolePagePermissionRepository rolePagePermissionRepository)
        {
            _appSetting = appSetting;
            _roleRepository = roleRepository;
            _pagePermissionRepository = pagePermissionRepository;
            _rolePagePermissionRepository = rolePagePermissionRepository;
        }
        #endregion

        #region Method
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(SetCompanyBasicRolesCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Role.Entity.Role superRole = new();
            superRole.CreateRole(_appSetting.SuperAdminRole, request.CompanyId);

            Domain.Role.Entity.Role normalRole = new();
            normalRole.CreateRole(_appSetting.NormalUserRole, request.CompanyId);

            _roleRepository.AddRole(superRole);
            _roleRepository.AddRole(normalRole);

            if (await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                await AddAllPageRolePermisionToSuperAdminRole(superRole.Id, cancellationToken);
                response.Result.ResponseData = true;
                response.Result.CommandMessage = "Role Saved Succefuly";
            }
            else
            {
                response.Result.CommandMessage = "Failed to add the new Role. Try again shortly.";
            }
            return response;
        }
        private async Task AddAllPageRolePermisionToSuperAdminRole(long roleId, CancellationToken cancellationToken)
        {
            var pagePermissions = await _pagePermissionRepository.GetAllPagePermissionsToSetCompanyBasicRole();
            if (pagePermissions?.Count > default(int))
            {
                foreach (var item in pagePermissions)
                {
                    RolePagePermission rolePagePermission = new();
                    rolePagePermission.CreatRolePagePermission(item.Id, roleId);
                    _rolePagePermissionRepository.AddPagePermissionRole(rolePagePermission);
                }
                await _rolePagePermissionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
        }
        #endregion
    }
}

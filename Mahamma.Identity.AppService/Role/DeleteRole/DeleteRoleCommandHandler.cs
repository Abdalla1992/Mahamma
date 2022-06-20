using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePagePermissionRepository _rolePagePermissionRepository;
        private readonly RoleManager<Domain.Role.Entity.Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly AppSetting _appSetting;


        #endregion

        #region Ctor
        public DeleteRoleCommandHandler(IRoleRepository roleRepository, IUserRoleRepository userRoleRepository
            , IRolePagePermissionRepository rolePagePermissionRepository, RoleManager<Domain.Role.Entity.Role> roleManager,
            AppSetting appSetting, UserManager<User> userManager)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rolePagePermissionRepository = rolePagePermissionRepository;
            _roleManager = roleManager;
            _appSetting = appSetting;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            Domain.Role.Entity.Role role = await _roleRepository.GetRoleById(request.RoleId);
            if (role != null)
            {
                role.DeleteRole();
                //_roleRepository.RemoveRole(role);
                _roleRepository.UpdateRole(role);

                List<Domain.UserRole.Entity.UserRole> usersRolesList = await _userRoleRepository.GetUsersByRoleId(request.RoleId);
                List<Domain.Role.Entity.RolePagePermission> pagePermissionsList = await _rolePagePermissionRepository.GetPagepermissionByRoleId(request.RoleId);

                if (pagePermissionsList?.Count > 0)
                {
                    _rolePagePermissionRepository.RemovePermissionList(pagePermissionsList);
                }

                if (usersRolesList?.Count > 0)
                {
                    _userRoleRepository.RemovePermissionRoleList(usersRolesList);

                    List<long> userIds = usersRolesList.Select(m => m.UserId).ToList();
                    if (userIds?.Count > 0)
                    {
                        foreach (var userid in userIds)
                        {
                            var user = _userManager.Users.FirstOrDefault(u => u.Id == userid);
                            if (user.CompanyId.HasValue)
                            {
                                Domain.Role.Entity.Role normalRole = await _roleRepository.GetRoleByNameAndCompany(_appSetting.NormalUserRole, user.CompanyId.Value);
                                Domain.UserRole.Entity.UserRole userRole = new();
                                userRole.CreatUserRoles(user.Id, normalRole.Id);
                                _userRoleRepository.AddUserRole(userRole);
                                await _userRoleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                                //await _userManager.AddToRoleAsync(user, _appSetting.NormalUserRole, (int)user.CompanyId, cancellationToken);
                            }
                        }
                    }
                }

                if (await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Role Deleted Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to delete This Role. Try again shortly.";

                }
            }
            else
            {
                response.Result.CommandMessage = "Role Not Found..";
            }
            return response;
        }
        #endregion
    }
}

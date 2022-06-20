using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Enum;
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

namespace Mahamma.Identity.AppService.Role.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly RoleManager<Domain.Role.Entity.Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePagePermissionRepository _rolePagePermissionRepository;
        private readonly AppSetting _appSetting;
        private readonly IPagePermissionRepository _pagePermissionRepository;

        #endregion

        #region Ctor
        public UpdateRoleCommandHandler(IUserRoleRepository userRoleRepository, IRoleRepository roleRepository,
            RoleManager<Domain.Role.Entity.Role> roleManager, IRolePagePermissionRepository rolePagePermissionRepository,
            AppSetting appSetting, UserManager<User> userManager, IPagePermissionRepository pagePermissionRepository)
        {
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _rolePagePermissionRepository = rolePagePermissionRepository;
            _appSetting = appSetting;
            _userManager = userManager;
            _pagePermissionRepository = pagePermissionRepository;
        }
        #endregion

        #region Methods
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Role.Entity.Role role = await _roleRepository.GetRoleById(request.Id);
            if (role != null)
            {

                role.UpdateRole(request.Name);
                List<Domain.UserRole.Entity.UserRole> usersRolesList = await _userRoleRepository.GetUsersByRoleId(request.Id);
                List<RolePagePermission> pagePermissionsList = await _rolePagePermissionRepository.GetPagepermissionByRoleId(request.Id);
                //AddOrUpdatePagePermission(request, pagePermissionsList);
                await UpdateRolePermissions(request, cancellationToken);
                await AddOrUpdateUser(request, usersRolesList, cancellationToken);

                if (await _userRoleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))// && (await _roleManager.UpdateAsync(role)).Succeeded)
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Permissions Added Successfully";
                }
                else
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Failed to add the new Permissions. Try again shortly.";
                }
            }
            else
            {
                response.Result.CommandMessage = "Permissions Not Found.";

            }
            return response;
        }

        private async Task AddOrUpdateUser(UpdateRoleCommand request, List<Domain.UserRole.Entity.UserRole> usersRolesList, CancellationToken cancellationToken)
        {
            if (usersRolesList?.Count > 0)
            {
                List<long> NewUsers = request.UserIds.Where(p => !usersRolesList.Any(m => m.UserId == p)).ToList();
                if (NewUsers?.Count > 0)
                {
                    Domain.UserRole.Entity.UserRole userRole = null;
                    foreach (var usersIds in NewUsers)
                    {

                        Domain.UserRole.Entity.UserRole ExistuserOnRole = await _userRoleRepository.GetUserRoleByUserId(usersIds);
                        if (ExistuserOnRole != null)
                        {
                            _userRoleRepository.RemoveUserRolee(ExistuserOnRole);
                        }

                        userRole = new();
                        userRole.CreatUserRoles(usersIds, request.Id);
                        _userRoleRepository.AddUserRole(userRole);
                    }
                }
                List<Domain.UserRole.Entity.UserRole> DeletedUsers = usersRolesList.Where(m => !request.UserIds.Contains(m.UserId)).ToList();
                if (DeletedUsers?.Count > 0)
                {
                    foreach (var deleted in DeletedUsers)
                    {
                        _userRoleRepository.RemoveUserRolee(deleted);
                    }

                    List<long> UserIds = DeletedUsers.Select(m => m.UserId).ToList();
                    if (UserIds?.Count > 0)
                    {
                        foreach (var deleteuserid in UserIds)
                        {
                            var user = _userManager.Users.FirstOrDefault(u => u.Id == deleteuserid);
                            Domain.Role.Entity.Role normalRole = await _roleRepository.GetRoleByNameAndCompany(_appSetting.NormalUserRole, user.CompanyId.Value);

                            Domain.UserRole.Entity.UserRole userrRole = await _userRoleRepository.GetUserRoleByRoleIdAndUserId(deleteuserid, normalRole.Id);
                            Domain.UserRole.Entity.UserRole userRole = new();
                            if (userrRole == null)
                            {
                                userRole.CreatUserRoles(user.Id, normalRole.Id);
                                _userRoleRepository.AddUserRole(userRole);
                                //await _userRoleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                            }

                            //_userManager.AddToRoleAsync(user, _appSetting.NormalUserRole, (int)user.CompanyId, cancellationToken);
                        }
                    }
                }
            }
            else
            {
                Domain.UserRole.Entity.UserRole UserRole = null;
                foreach (var userId in request.UserIds)
                {
                    Domain.UserRole.Entity.UserRole ExistuserOnRole = await _userRoleRepository.GetUserRoleByUserId(userId);
                    if (ExistuserOnRole != null)
                    {
                        _userRoleRepository.RemoveUserRolee(ExistuserOnRole);
                    }

                    UserRole = new();
                    UserRole.CreatUserRoles(userId, request.Id);
                    _userRoleRepository.AddUserRole(UserRole);
                }
            }
        }

        private void AddOrUpdatePagePermission(UpdateRoleCommand request, List<RolePagePermission> permissionsRolesList)
        {
            int addMeetingId = Permission.AddMeeting.Id;
            RolePagePermission rolePagePermissionRole = null;
            if (permissionsRolesList?.Count > 0)
            {
                List<int> NewPermissiom = request.PagePermissionIds.Where(p => !permissionsRolesList.Any(m => m.PagePermissionId == p)).ToList();
                if (NewPermissiom?.Count > 0)
                {
                    if (NewPermissiom.Contains(Permission.AddMeetingGeneral.Id) || NewPermissiom.Contains(Permission.AddMeetingProject.Id) || NewPermissiom.Contains(Permission.AddMeetingWorkspace.Id) || NewPermissiom.Contains(Permission.AddMeetingTask.Id))
                    {
                        rolePagePermissionRole = new();
                        rolePagePermissionRole.CreatRolePagePermission(addMeetingId, request.Id);
                        _rolePagePermissionRepository.AddPagePermissionRole(rolePagePermissionRole);
                    }
                    foreach (var pagePermissionsIds in NewPermissiom)
                    {
                        rolePagePermissionRole = new();
                        rolePagePermissionRole.CreatRolePagePermission(pagePermissionsIds, request.Id);
                        _rolePagePermissionRepository.AddPagePermissionRole(rolePagePermissionRole);
                    }
                }
                List<RolePagePermission> DeletedPermission = permissionsRolesList.Where(m => !request.PagePermissionIds.Contains(m.PagePermissionId)).ToList();
                if (DeletedPermission?.Count > 0)
                {
                    _rolePagePermissionRepository.RemovePermissionList(DeletedPermission);
                }
            }
            else
            {
                RolePagePermission rolePagePermission = null;
                //int addMeetingId = Permission.AddMeeting.Id;
                if (request.PagePermissionIds.Contains(Permission.AddMeetingGeneral.Id) || request.PagePermissionIds.Contains(Permission.AddMeetingProject.Id) || request.PagePermissionIds.Contains(Permission.AddMeetingWorkspace.Id) || request.PagePermissionIds.Contains(Permission.AddMeetingTask.Id))
                {
                    rolePagePermission = new();
                    rolePagePermission.CreatRolePagePermission(addMeetingId, request.Id);
                    _rolePagePermissionRepository.AddPagePermissionRole(rolePagePermission);
                }
                foreach (var permissionId in request.PagePermissionIds)
                {
                    rolePagePermission = new();
                    rolePagePermission.CreatRolePagePermission(permissionId, request.Id);
                    _rolePagePermissionRepository.AddPagePermissionRole(rolePagePermission);
                }
            }
        }

        private async Task UpdateRolePermissions( UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            List<RolePagePermission> rolePagePermissions = await _rolePagePermissionRepository.GetPagepermissionByRoleId(request.Id);
            if (rolePagePermissions?.Count > 0)
            {
                _rolePagePermissionRepository.RemovePermissionList(rolePagePermissions);
            }
            List<int> addMeetingIds = Permission.AddMeetingList().Select(p => p.Id).ToList();
            List<PagePermission> addMeetingPagePermissions = await _pagePermissionRepository.GetPagePermissionsByPermissionIds(addMeetingIds);
            if (addMeetingPagePermissions?.Count > 0 &&
                addMeetingPagePermissions.Any(p => request.PagePermissionIds.Contains(p.Id)))
            {
                List<int> addMeetingPagePermissionIds = await _pagePermissionRepository.GetPagePermissionsIdsByPermissionId(Permission.AddMeeting.Id);
                if (addMeetingPagePermissionIds?.Count > 0)
                {
                    request.PagePermissionIds.AddRange(addMeetingPagePermissionIds);
                }
            }
            RolePagePermission rolePagePermissionRole = null;
            foreach (var pagePermissionsIds in request.PagePermissionIds)
            {
                rolePagePermissionRole = new();
                rolePagePermissionRole.CreatRolePagePermission(pagePermissionsIds, request.Id);
                _rolePagePermissionRepository.AddPagePermissionRole(rolePagePermissionRole);
            }
            //await _rolePagePermissionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
        #endregion
    }
}

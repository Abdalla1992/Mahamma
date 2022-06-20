using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Enum;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.SetCompaany
{
    public class SetCompanyCommandHandler : IRequestHandler<SetCompanyCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        // private readonly CustomUserManager _userManager;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public SetCompanyCommandHandler(IUserRepository userRepository, UserManager<User> userManager,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, AppSetting appSetting, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContext = httpContext;
            _appSetting = appSetting;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(SetCompanyCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            long userId = (long)_httpContext.HttpContext.Items["UserId"];

            User user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                user.SetCompany(request.CompanyId, UserProfileStatus.CompanyCreated.Id);
                if ((await _userManager.UpdateAsync(user)).Succeeded)
                {
                    Domain.Role.Entity.Role role = await _roleRepository.GetRoleByNameAndCompany(_appSetting.SuperAdminRole, request.CompanyId);
                    Domain.UserRole.Entity.UserRole userRole = new();
                    userRole.CreatUserRoles(user.Id, role.Id);
                    _userRoleRepository.AddUserRole(userRole);
                    await _userRoleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Modified";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to modify";
                }
            }
            else
            {
                response.Result.CommandMessage = "User Not Found";
            }

            return response;
        }
    }
}

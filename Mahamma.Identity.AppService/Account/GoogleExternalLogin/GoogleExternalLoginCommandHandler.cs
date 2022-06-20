using Mahamma.ApiClient.Dto.Company;
using Mahamma.ApiClient.Interface;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.AppService.Account.Helper;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Enum;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.GoogleExternalLogin
{
    public class GoogleExternalLoginCommandHandler : IRequestHandler<GoogleExternalLoginCommand, ValidateableResponse<ApiResponse<UserDto>>>
    {
        private readonly IJWTHelper _jwtHelper;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyService _companyService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly AppSetting _appSetting;
        public GoogleExternalLoginCommandHandler(IJWTHelper jwtHelper, UserManager<User> userManager,
            IUserRepository userRepository, AppSetting appSetting, IUserRoleRepository userRoleRepository,
            ICompanyService companyService, IRoleRepository roleRepository)
        {
            _jwtHelper = jwtHelper;
            _userManager = userManager;
            _userRepository = userRepository;
            _companyService = companyService;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _appSetting = appSetting;
        }
        public async Task<ValidateableResponse<ApiResponse<UserDto>>> Handle(GoogleExternalLoginCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<UserDto>> response = new(new ApiResponse<UserDto>());
            var payload = await _jwtHelper.VerifyGoogleToken(request.Provider, request.IdToken);
            if (payload != null)
            {
                var info = new UserLoginInfo(request.Provider, payload.Subject, request.FullName);
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(payload.Email);
                    //register
                    if (user == null)
                    {
                        user = new User();
                        if (!string.IsNullOrWhiteSpace(request.InvitationId))
                        {
                            CompanyInvitationDto companyInvitationDto = await _companyService.GetCompanyInvitation(request.InvitationId);
                            user.CreateUser(payload.Email, UserProfileStatus.Registered.Id, companyInvitationDto?.CompanyId);
                        }
                        else
                        {
                            user.CreateUser(payload.Email, UserProfileStatus.Registered.Id);
                        }

                        var result = await _userManager.CreateAsync(user);
                        if (result.Succeeded)
                        {
                            await _userManager.AddLoginAsync(user, info);
                            if (!string.IsNullOrWhiteSpace(request.InvitationId))
                            {
                                Domain.Role.Entity.Role role = await _roleRepository.GetRoleByNameAndCompany(_appSetting.NormalUserRole, user.CompanyId.Value);
                                Domain.UserRole.Entity.UserRole userRole = new();
                                userRole.CreatUserRoles(user.Id, role.Id);
                                _userRoleRepository.AddUserRole(userRole);
                                await _userRoleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                                //await _userManager.AddToRoleAsync(user, _appSetting.NormalUserRole, (int)user.CompanyId, cancellationToken);
                            }
                        }
                    }
                    //login
                    else
                    {
                        await _userManager.AddLoginAsync(user, info);
                    }
                }
                if (user != null)
                {
                    response.Result.ResponseData = _userRepository.MapUserToUserDto(user);
                    response.Result.ResponseData.AuthToken = "Bearer " + _jwtHelper.GenerateNewJWT(user);
                    response.Result.CommandMessage = "Authinticated";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to authinticate";
                }
            }
            else
            {
                response.Result.CommandMessage = "Failed to authinticate";
            }
            return response;
        }
    }
}

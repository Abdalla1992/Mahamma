using Mahamma.ApiClient.Dto.Company;
using Mahamma.ApiClient.Interface;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.AppService.Account.Helper;
using Mahamma.Identity.AppService.Setting;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Enum;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Domain.UserRole.Entity;
using Mahamma.Identity.Domain.UserRole.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.AddComment
{
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, ValidateableResponse<ApiResponse<UserDto>>>
    {
        #region Props
        public UserManager<User> _userManager { get; set; }
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        private readonly AppSetting _appSetting;
        private readonly ICompanyService _companyService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        #endregion

        #region ctor
        public RegistrationCommandHandler(UserManager<User> userManager, IRoleRepository roleRepository, SignInManager<User> signInManager, IJWTHelper jwtHelper,
            IUserRepository userRepository, AppSetting appSetting, ICompanyService companyService, IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
            _userRepository = userRepository;
            _appSetting = appSetting;
            _companyService = companyService;
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<UserDto>>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<UserDto>> response = new(new ApiResponse<UserDto>());
            User user = new();
            if (!string.IsNullOrWhiteSpace(request.InvitationId))
            {
                CompanyInvitationDto companyInvitationDto = await _companyService.GetCompanyInvitation(request.InvitationId);
                user.CreateUser(request.Email, UserProfileStatus.Registered.Id, companyInvitationDto?.CompanyId);
            }
            else
            {
                user.CreateUser(request.Email, UserProfileStatus.Registered.Id);
            }
            var registerResult = await _userManager.CreateAsync(user, request.Password);
            if (registerResult.Succeeded)
            {
                var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, true, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(request.InvitationId))
                    {
                        Role role = await _roleRepository.GetRoleByNameAndCompany(_appSetting.NormalUserRole, user.CompanyId.Value);
                        UserRole userRole = new();
                        userRole.CreatUserRoles(user.Id, role.Id);
                        _userRoleRepository.AddUserRole(userRole);
                        await _userRoleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                        //await _userManager.AddToRoleAsync(user, _appSetting.NormalUserRole, (int)user.CompanyId, cancellationToken);
                    }
                    response.Result.ResponseData = _userRepository.MapUserToUserDto(user);
                    response.Result.ResponseData.AuthToken = "Bearer " + _jwtHelper.GenerateNewJWT(user);
                    response.Result.CommandMessage = "User Registered Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to register, please try again shortly";
                }
            }
            else
            {
                response.Result.CommandMessage = string.Join(',', registerResult.Errors.Select(e => e.Description).ToList());
            }
            return response;
        }
    }
}

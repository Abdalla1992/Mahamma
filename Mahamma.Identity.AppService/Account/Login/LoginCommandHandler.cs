using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.AppService.Account.Helper;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.Login.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ValidateableResponse<ApiResponse<UserDto>>>
    {
        #region Props
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        #endregion

        #region ctor
        public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IJWTHelper jwtHelper, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
            _userRepository = userRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<UserDto>>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<UserDto>> response = new(new ApiResponse<UserDto>());
            User user = await _userRepository.GetUserByEmail(request.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, true, false);

                if (result.Succeeded)
                {
                    response.Result.ResponseData = _userRepository.MapUserToUserDto(user);
                    response.Result.ResponseData.AuthToken = "Bearer " + _jwtHelper.GenerateNewJWT(user);
                    response.Result.CommandMessage = "Authinticated";
                }
                else
                {
                    response.Result.CommandMessage = "Email or password is not correct";
                }
            }
            else
            {
                response.Result.CommandMessage = "Email or password is not correct";
            }
            return response;
        }
    }
}

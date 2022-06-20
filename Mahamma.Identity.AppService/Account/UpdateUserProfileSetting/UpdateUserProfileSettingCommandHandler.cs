using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileSetting
{
    public class UpdateUserProfileSettingCommandHandler : IRequestHandler<UpdateUserProfileSettingCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public UpdateUserProfileSettingCommandHandler(IUserRepository userRepository, UserManager<User> userManager,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContext = httpContext;

        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateUserProfileSettingCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            long currentUser = (long)_httpContext.HttpContext.Items["UserId"];
            User user = await _userRepository.GetUserById(currentUser);
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(request.CurrentPassword) && !string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                }
                string skills = request.Skills?.Count > 0 ? string.Join(',', request.Skills) : null;
                user.UpdateUserProfileSetting(request.ProfileImage, request.FullName, request.JobTitle,
                 request.Email, request.LanguageId, request.Bio, skills);
                if ((await _userManager.UpdateAsync(user)).Succeeded)
                {
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

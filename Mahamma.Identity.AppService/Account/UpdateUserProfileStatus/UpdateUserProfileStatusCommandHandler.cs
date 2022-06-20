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

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileStatus
{
    public class UpdateUserProfileStatusCommandHandler : IRequestHandler<UpdateUserProfileStatusCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;

        public UpdateUserProfileStatusCommandHandler(IUserRepository userRepository, UserManager<User> userManager, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContext = httpContext;
        }
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateUserProfileStatusCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            long userId = (long)_httpContext.HttpContext.Items["UserId"];

            User user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                user.UpdateUserProfileStatus(request.UserProfileStatusId);

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

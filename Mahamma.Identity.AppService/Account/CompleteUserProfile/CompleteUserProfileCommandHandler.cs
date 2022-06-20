using Mahamma.ApiClient.Dto.Base;
using Mahamma.ApiClient.Interface;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Enum;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.CompleteUserProfile
{
    public class CompleteUserProfileCommandHandler : IRequestHandler<CompleteUserProfileCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly ICompanyService _companyService;

        public CompleteUserProfileCommandHandler(IUserRepository userRepository, UserManager<User> userManager,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, ICompanyService companyService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _httpContext = httpContext;
            _companyService = companyService;
        }
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(CompleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            long userId = (long)_httpContext.HttpContext.Items["UserId"];
            var user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                user.UpdateUser(request.ProfileImage, request.FullName, request.JobTitle, request.WorkingDays, request.WorkingHours, !string.IsNullOrWhiteSpace(request.InvitationId) ? UserProfileStatus.FirstWorkspaceCreated.Id : UserProfileStatus.ProfileCompleted.Id);

                if ((await _userManager.UpdateAsync(user)).Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(request.InvitationId))
                    {
                        if (await _companyService.UpdateInvitationStatus(new BaseRequestDto() { AuthToken = GetAccessToken() }, request.InvitationId, InvitationStatus.Opened.Id))
                        {
                            response.Result.ResponseData = true;
                            response.Result.CommandMessage = "Data Modified";
                        }
                        else
                        {
                            response.Result.CommandMessage = "Data Modified But Invitation Not Closed";
                        }
                    }
                    else
                    {
                        response.Result.ResponseData = true;
                        response.Result.CommandMessage = "Data Modified";
                    }
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

        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }
    }
}

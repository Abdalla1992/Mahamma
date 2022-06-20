using Mahamma.ApiClient.Dto.Company;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.Domain.Notification.Dto;
using Mahamma.Notification.Domain.Notification.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.GetNotificationsCount
{
    public class GetNotificationsCountCommand : IRequest<ValidateableResponse<ApiResponse<int>>>
    {
    }
    public class GetNotificationsCountCommandHandler : IRequestHandler<GetNotificationsCountCommand, ValidateableResponse<ApiResponse<int>>>
    {
        #region Prop
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public GetNotificationsCountCommandHandler(IHttpContextAccessor httpContext, IAccountService accountService, INotificationRepository notificationRepository)
        {
            _httpContext = httpContext;
            _accountService = accountService;
            _notificationRepository = notificationRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<int>>> Handle(GetNotificationsCountCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<int>> response = new(new ApiResponse<int>());

            string currentUserId = (string)_httpContext.HttpContext.Items["UserId"];
            long userId=default;
            bool result = long.TryParse(currentUserId,out userId);
            if (result)
            {
                int notificationCount = await _notificationRepository.GetNotificationCount(n => n.IsRead == false && n.ReceiverUserId==userId);
                if (notificationCount > 0)
                {
                    response.Result.CommandMessage = $"{notificationCount} notification found";
                    response.Result.ResponseData = notificationCount;
                }
                else
                {
                    response.Result.CommandMessage = $"No date found.";
                }
            }
            else
            {
                response.Result.CommandMessage = $"No date found.";
            }
            return response;
        }
        private List<MemberDto> SetMembers(params long[] userIdList)
        {
            var result = new List<MemberDto>();
            foreach (var userId in userIdList)
            {
                UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, userId);
                if (userDto != null)
                {
                    result.Add(new MemberDto
                    {
                        UserId = userDto.Id,
                        FullName = userDto.FullName,
                        ProfileImage = userDto.ProfileImage
                    });
                }
            }
            return result;
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

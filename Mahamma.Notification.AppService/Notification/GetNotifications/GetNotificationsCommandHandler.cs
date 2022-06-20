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

namespace Mahamma.Notification.AppService.Notification.GetNotifications
{
    public class GetNotificationsCommand : IRequest<ValidateableResponse<ApiResponse<List<NotificationContentDto>>>>
    {
    }
    public class GetNotificationsCommandHandler : IRequestHandler<GetNotificationsCommand, ValidateableResponse<ApiResponse<List<NotificationContentDto>>>>
    {
        #region Prop
        private readonly INotificationContentRepository _notificationContentRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public GetNotificationsCommandHandler(INotificationContentRepository notificationContentRepository, IHttpContextAccessor httpContext, IAccountService accountService)
        {
            _notificationContentRepository = notificationContentRepository;
            _httpContext = httpContext;
            _accountService = accountService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<List<NotificationContentDto>>>> Handle(GetNotificationsCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<List<NotificationContentDto>>> response = new(new ApiResponse<List<NotificationContentDto>>());

            string currentUserId = (string)_httpContext.HttpContext.Items["UserId"];

            PageList<NotificationContentDto> notificationList = await _notificationContentRepository.GetMappedNotificationList(0,1000, n => n.Notification.ReceiverUserId == long.Parse(currentUserId), "Notification");
            foreach (var item in notificationList.DataList)
            {
                item.Notification.ReceiverMember = SetMembers(item.Notification.SenderUserId).FirstOrDefault();
                item.Notification.NotificationContents = null;
            }
            if (notificationList != null && notificationList.DataList.Count > 0)
            {
                response.Result.CommandMessage = $"{notificationList.DataList.Count} notification found";
                response.Result.ResponseData = notificationList.DataList;
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

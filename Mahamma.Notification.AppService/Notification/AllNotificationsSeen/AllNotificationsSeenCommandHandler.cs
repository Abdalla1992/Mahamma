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
    public class AllNotificationsSeenCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
    }
    public class AllNotificationsSeenCommandHandler : IRequestHandler<AllNotificationsSeenCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly INotificationContentRepository _notificationContentRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public AllNotificationsSeenCommandHandler(INotificationContentRepository notificationContentRepository, IHttpContextAccessor httpContext, INotificationRepository notificationRepository)
        {
            _notificationContentRepository = notificationContentRepository;
            _httpContext = httpContext;
            _notificationRepository = notificationRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AllNotificationsSeenCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            string currentUserId = (string)_httpContext.HttpContext.Items["UserId"];

            PageList<Domain.Notification.Entity.Notification> notificationList = await _notificationRepository.GetNotificationList(0,1000, n => n.ReceiverUserId == long.Parse(currentUserId));
            foreach (var item in notificationList.DataList)
            {
                item.IsRead = true;
            }
            if (await _notificationContentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = $"all seen";
            }
            else
            {
                response.Result.CommandMessage = $"No date found.";
            }
            return response;
        }
    }
}

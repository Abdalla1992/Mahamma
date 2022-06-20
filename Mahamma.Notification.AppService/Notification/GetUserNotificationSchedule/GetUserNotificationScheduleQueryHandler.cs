using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Notification.Domain.Notification.Dto;
using Mahamma.Notification.Domain.Notification.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.GetUserNotificationSchedule
{
   
    public class GetUserNotificationScheduleQueryHandler : IRequestHandler<GetUserNotificationScheduleQuery, ValidateableResponse<ApiResponse<NotificationScheduleDto>>>
    {
        #region Prop
        private readonly INotificationSheduleRepository _notificationSheduleRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;

        #endregion
        public GetUserNotificationScheduleQueryHandler(INotificationSheduleRepository notificationSheduleRepository,Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _notificationSheduleRepository = notificationSheduleRepository;
            _httpContext = httpContext;
        }
        public async Task<ValidateableResponse<ApiResponse<NotificationScheduleDto>>> Handle(GetUserNotificationScheduleQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<NotificationScheduleDto>> response = new(new ApiResponse<NotificationScheduleDto>());
            //string currentUserId = (string)_httpContext.HttpContext.Items["UserId"];
            NotificationScheduleDto notificationScheduleDto = await _notificationSheduleRepository.GetUsernotificationShedule(request.UserId);

            if (notificationScheduleDto != null)
            {
                response.Result.ResponseData = notificationScheduleDto;
                response.Result.CommandMessage = "Data Found Success";
            }
            else
            {
                response.Result.CommandMessage = "No Notiication Setting To This User";
            }
            return response;
        }
    }


}

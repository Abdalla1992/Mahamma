using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Notification.AppService.Notification.AddNotification;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.AddNotificationShedule
{
    public class AddNotificationsheduleCommandHandler : IRequestHandler<AddNotificationScheduleCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly INotificationSheduleRepository _notificationSheduleRepository;
        #endregion

        #region Ctor
        public AddNotificationsheduleCommandHandler(INotificationSheduleRepository notificationSheduleRepository)
        {
            _notificationSheduleRepository = notificationSheduleRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddNotificationScheduleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            NotificationShedule notificationScheduleCheck= await _notificationSheduleRepository.GetnotificationSheduleByUserId(request.UserId);
            if (notificationScheduleCheck == null)
            {
                NotificationShedule notificationShedule = new();
                notificationShedule.CreateNotificationShedule(request.From, request.To, request.UserId, request.NotificationScheduleTypeId, request.WeekDayId, request.MonthDayId);
                _notificationSheduleRepository.CreateNotificationSchedule(notificationShedule);
                if (await _notificationSheduleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Added Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to add the new NotificationSchedule. Try again shortly.";
                }
            }
            else
            {
                response.Result.CommandMessage = "This User Have Setting Already.";
            }

            return response;
        }
    }
}

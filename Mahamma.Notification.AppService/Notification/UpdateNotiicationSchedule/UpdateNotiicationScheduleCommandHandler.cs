using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.UpdateNotiicationSchedule
{
    public class UpdateNotiicationScheduleCommandHandler : IRequestHandler<UpdateNotiicationScheduleCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly INotificationSheduleRepository _notificationSheduleRepository;
        #endregion

        #region Ctor
        public UpdateNotiicationScheduleCommandHandler(INotificationSheduleRepository notificationSheduleRepository)
        {
            _notificationSheduleRepository = notificationSheduleRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateNotiicationScheduleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            NotificationShedule notiicationSchedule = await _notificationSheduleRepository.GetEntityById(request.Id);
            if (notiicationSchedule != null)
            {
                notiicationSchedule.UpdateNotificationShedule(request.From, request.To, request.UserId, request.NotificationScheduleTypeId, request.WeekDayId, request.MonthDayId);
                _notificationSheduleRepository.UpdateNotificationShedule(notiicationSchedule);
                if (await _notificationSheduleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Saved Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Faild To Modify";
                }
            }
            else
            {
                response.Result.CommandMessage = "No Notification Schedule Found";
            }
            return response;
        }
    }
}

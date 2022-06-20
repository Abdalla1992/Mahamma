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

namespace Mahamma.Notification.AppService.Notification.DeleteNotificationSchedule
{
    public class DeleteNotificationScheduleCommandHandler : IRequestHandler<DeleteNotificationScheduleCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly INotificationSheduleRepository _notificationSheduleRepository;
        #endregion

        #region Ctor
        public DeleteNotificationScheduleCommandHandler(INotificationSheduleRepository notificationSheduleRepository)
        {
            _notificationSheduleRepository = notificationSheduleRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteNotificationScheduleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            NotificationShedule notificationSchedule = await _notificationSheduleRepository.GetEntityById(request.Id);
            if (notificationSchedule != null)
            {
                notificationSchedule.DeleteNotificationShedule();
                _notificationSheduleRepository.UpdateNotificationShedule(notificationSchedule);
                if (await _notificationSheduleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Reset Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Faild To Reset";
                }
            }
            else
            {
                response.Result.CommandMessage = "No Data Found!";
            }
            return response;
        }
    }
}

using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Notification.Domain.Notification.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.AddNotification
{
    public class AddNotificationCommandHandler : IRequestHandler<AddNotificationCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly INotificationRepository _notificationRepository;
 
        #endregion

        #region Ctor
        public AddNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Notification.Entity.Notification notification = new();
            notification.CreateNotification(null, null, null, null,
             request.NotificationSendingTypeId, request.NotificationSendingStatusId, request.NotificationTypeId, request.SenderUserId, request.ReceiverUserId, request.IsRead, null);
            _notificationRepository.AddNotification(notification);
            if (await _notificationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = "Data Added Successfully";
            }
            else
            {
                response.Result.CommandMessage = "Failed to add the new Notification. Try again shortly.";
            }
            return response;
        }
    }
}

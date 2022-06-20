using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Notification.Api.Hub;
using Mahamma.Notification.AppService.Notification.Helper.WorkerServiceParallelHelper;
using Mahamma.Notification.AppService.Settings;
using Mahamma.Notification.Domain.Notification.Enum;
using Mahamma.Notification.Domain.Notification.Repository;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.TemporaryAppService
{
    public class SendSignalRNotification : ISendSignalRNotification
    {
        #region Prop
        private readonly IHubContext<NotifyHub, IHubClient> _notifyHub;
        private readonly IWorkerServiceParallelHelper _workerServiceParallelHelper;
        private readonly INotificationContentRepository _notificationContentRepository;
        private readonly BackGroundServiceSettings _backGroundServiceSettings;
        private readonly INotificationRepository _notificationRepository;
        #endregion

        #region Ctor
        public SendSignalRNotification(IHubContext<NotifyHub, IHubClient> notifyHub, IWorkerServiceParallelHelper workerServiceParallelHelper, INotificationContentRepository notificationContentRepository, BackGroundServiceSettings backGroundServiceSettings, INotificationRepository notificationRepository)
        {
            _notifyHub = notifyHub;
            _workerServiceParallelHelper = workerServiceParallelHelper;
            _notificationContentRepository = notificationContentRepository;
            _backGroundServiceSettings = backGroundServiceSettings;
            _notificationRepository = notificationRepository;
        }
        #endregion
        public async Task<bool> Handle(int skipCount, CancellationToken cancellationToken)
        {
            PageList<Domain.Notification.Entity.Notification> notifiactionList = await _notificationRepository.GetNotificationList(skipCount, _backGroundServiceSettings.TakeCount,
                n => !n.IsRead && n.NotificationSendingStatusId == NotificationSendingStatus.New.Id 
                && n.NotificationSendingTypeId == NotificationSendingType.PushNotification.Id);

            if (notifiactionList != null && notifiactionList.DataList.Any())
            {
                await _workerServiceParallelHelper.HandleProcess(notifiactionList.DataList, HandleSignalRNotifications,
                    cancellationToken, _backGroundServiceSettings.UseParallel, _backGroundServiceSettings.ThreadCount);

                await _notificationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            return true;
        }
        private async Task HandleSignalRNotifications(Domain.Notification.Entity.Notification notification, int notificationCount, CancellationTokenSource cancellationTokenSource)
        {
            await _notifyHub.Clients.User(notification.ReceiverUserId.ToString()).InformClient(notificationCount);
            notification.UpdateNotificationSendingStatus(NotificationSendingStatus.Sent.Id);
            _notificationRepository.UpdateNotification(notification);
        }
    }
}

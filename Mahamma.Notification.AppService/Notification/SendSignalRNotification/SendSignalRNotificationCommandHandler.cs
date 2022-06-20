using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Helper.EmailSending.IService;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.AppService.Notification.Helper;
using Mahamma.Notification.AppService.Settings;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Enum;
using Mahamma.Notification.Domain.Notification.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.SendSignalRNotification
{
    public class SendSignalRNotificationCommand : IRequest<bool>
    {
        [DataMember]
        public int SkipCount { get; set; }
        public SendSignalRNotificationCommand(int skipCount)
        {
            SkipCount = skipCount;
        }
    }
    public class SendSignalRNotificationCommandHandler : IRequestHandler<SendSignalRNotificationCommand, bool>
    {
        #region Prop
        private readonly IHubContext<NotifyHub, IHubClient> _notifyHub;
        private readonly IWorkerServiceParallelHelper _workerServiceParallelHelper;
        private readonly INotificationContentRepository _notificationContentRepository;
        private readonly BackGroundServiceSettings _backGroundServiceSettings;
        private readonly INotificationRepository _notificationRepository;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public SendSignalRNotificationCommandHandler(IHubContext<NotifyHub, IHubClient> notifyHub, IWorkerServiceParallelHelper workerServiceParallelHelper, INotificationContentRepository notificationContentRepository, BackGroundServiceSettings backGroundServiceSettings, INotificationRepository notificationRepository, IAccountService accountService)
        {
            _notifyHub = notifyHub;
            _workerServiceParallelHelper = workerServiceParallelHelper;
            _notificationContentRepository = notificationContentRepository;
            _backGroundServiceSettings = backGroundServiceSettings;
            _notificationRepository = notificationRepository;
            _accountService = accountService;
        }
        #endregion


        public async Task<bool> Handle(SendSignalRNotificationCommand request, CancellationToken cancellationToken)
        {
            PageList<NotificationContent> notifiactionList =await _notificationContentRepository.GetNotificationList(request.SkipCount, _backGroundServiceSettings.TakeCount,
                n => n.Notification.NotificationSendingStatusId == NotificationSendingStatus.New.Id &&
                (n.Notification.NotificationSendingTypeId == NotificationSendingType.PushNotification.Id ||
                n.Notification.NotificationSendingTypeId == NotificationSendingType.All.Id), "Notification");
            if (notifiactionList != null && notifiactionList.DataList.Any())
            {
                await _workerServiceParallelHelper.HandleProcess(notifiactionList.DataList, HandleSignalRNotifications,
                    cancellationToken, _backGroundServiceSettings.UseParallel, _backGroundServiceSettings.ThreadCount);

                await _notificationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            return true;
        }
        private async Task HandleSignalRNotifications(NotificationContent notification, CancellationTokenSource cancellationTokenSource)
        {

            UserDto userDto = _accountService.GetUserByIdForBackgroundService(notification.Notification.ReceiverUserId);
            IHubClients<IHubClient> client = _notifyHub.Clients;
            await _notifyHub.Clients.User(userDto.UserName).InformClient("hello");
            //await _notifyHub.Clients.All.InformClient("hello");
            //notification.Notification.UpdateNotificationSendingStatus(NotificationSendingStatus.Sent.Id);
            //_notificationRepository.UpdateNotification(notification.Notification);
        }
    }
}

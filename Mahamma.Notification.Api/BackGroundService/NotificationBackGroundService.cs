using Mahamma.Notification.Api.TemporaryAppService;
using Mahamma.Notification.AppService.Notification.SendDesktopNotification;
using Mahamma.Notification.AppService.Notification.SendNotificationByEmail;
using Mahamma.Notification.AppService.Settings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.BackGroundService
{
    public class NotificationBackGroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public readonly BackGroundServiceSettings _backGroundServiceSettings;
        private readonly ISendSignalRNotification _sendSignalRNotification;


        public NotificationBackGroundService(IServiceScopeFactory serviceScopeFactory, BackGroundServiceSettings backGroundServiceSettings, ISendSignalRNotification sendSignalRNotification)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _backGroundServiceSettings = backGroundServiceSettings;
            _sendSignalRNotification = sendSignalRNotification;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                int notificationCounter = 0;
                int notificationSkipCount = 0;

                while (!stoppingToken.IsCancellationRequested)
                {
                    notificationSkipCount = _backGroundServiceSettings.TakeCount * notificationCounter++;
                    try
                    {
                        var isSignalRNotificationsSent = _sendSignalRNotification.Handle(notificationSkipCount, stoppingToken);
                        var totalHandledNotificationCount = mediator.Send(new SendNotificationByEmailCommand(notificationSkipCount), stoppingToken);
                        var totalHandledDesktopNotificationCount = mediator.Send(new SendDesktopNotificationCommand(notificationSkipCount), stoppingToken);
                        if (await totalHandledNotificationCount <= 0 && await isSignalRNotificationsSent && await totalHandledDesktopNotificationCount <= 0) 
                            notificationCounter = 0;
                    }
                    catch (Exception ex)
                    {

                        var message = ex.Message;
                        Console.WriteLine(message);
                    }
                    await Task.Delay(_backGroundServiceSettings.WorkEveryInMilliseconds, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                Console.WriteLine(message);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}

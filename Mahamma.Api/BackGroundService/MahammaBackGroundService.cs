using Mahamma.AppService.Company.SendInvitation;
using Mahamma.AppService.Settings;
using Mahamma.AppService.Task.TaskStatusTracker;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Api.BackGroundService
{
    public class MahammaBackGroundService : BackgroundService
    {
        private readonly IMediator _mediator;
        public readonly BackGroundServiceSettings _backGroundServiceSettings;
        public MahammaBackGroundService(BackGroundServiceSettings backGroundServiceSettings, IMediator mediator)
        {
            _backGroundServiceSettings = backGroundServiceSettings;
            _mediator = mediator;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Backgroudn Servcie is starting");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                int taskCounter = 0;
                int invitationCounter = 0;
                int taskSkipCount = 0;
                int invitationSkipCount = 0;
                while (!stoppingToken.IsCancellationRequested)
                {
                    taskSkipCount = _backGroundServiceSettings.TakeCount * taskCounter++;
                    invitationSkipCount = _backGroundServiceSettings.TakeCount * invitationCounter++;
                    int totalHandledTasksCount = 0;
                    int totalSentInvitationCount = 0;
                    try
                    {
                        totalHandledTasksCount = _mediator.Send(new TaskStatusTrackerCommand(taskSkipCount), stoppingToken).Result;
                        totalSentInvitationCount = _mediator.Send(new SendInvitationCommand(invitationSkipCount), stoppingToken).Result;

                        if (totalHandledTasksCount <= 0)
                            taskCounter = 0;

                        if (totalSentInvitationCount <= 0)
                            invitationCounter = 0;
                    }
                    catch (Exception exception)
                    {
                        if (totalHandledTasksCount <= 0)
                            taskCounter = 0;

                        if (totalSentInvitationCount <= 0)
                            invitationCounter = 0;
                        Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
                    }
                    await Task.Delay(_backGroundServiceSettings.WorkEveryInMilliseconds, stoppingToken);
                }
            }
            catch (Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Error("Backgroudn Servcie is stopping");
            return base.StopAsync(cancellationToken);
        }
    }
}

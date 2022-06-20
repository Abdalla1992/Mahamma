using Mahamma.AppService.Settings;
using Mahamma.AppService.Task.Helper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Task.Event;
using Mahamma.Domain.Task.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskStatus = Mahamma.Domain.Task.Enum.TaskStatus;

namespace Mahamma.AppService.Task.TaskStatusTracker
{
    public class TaskStatusTrackerHandler : IRequestHandler<TaskStatusTrackerCommand, int>
    {
        #region Props
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;
        private readonly IWorkerServiceParallelHelper _workerServiceParallelHelper;
        private readonly BackGroundServiceSettings _backGroundServiceSettings;
        public List<TaskInProgressEvent> _events { get; set; }
        #endregion

        #region CTRS
        public TaskStatusTrackerHandler(IServiceProvider serviceProvider, IWorkerServiceParallelHelper workerServiceParallelHelper, BackGroundServiceSettings backGroundServiceSettings, IMediator mediator)
        {
            _taskRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ITaskRepository>();
            _workerServiceParallelHelper = workerServiceParallelHelper;
            _backGroundServiceSettings = backGroundServiceSettings;
            _mediator = mediator;
            _events = new List<TaskInProgressEvent>();
        }
        #endregion

        public async Task<int> Handle(TaskStatusTrackerCommand request, CancellationToken cancellationToken)
        {
            PageList<Domain.Task.Entity.Task> taskList = await _taskRepository.GetTaskList(request.SkipCount, _backGroundServiceSettings.TakeCount,
                t => t.TaskStatusId != TaskStatus.CompletedEarly.Id && t.TaskStatusId != TaskStatus.CompletedOnTime.Id && t.TaskStatusId != TaskStatus.CompletedLate.Id);
            if (taskList != null && taskList.DataList.Any())
            {
                try
                {
                        _workerServiceParallelHelper.HandleProcess(taskList.DataList, HandleTaskStatusTraking,
                            cancellationToken, _backGroundServiceSettings.UseParallel, _backGroundServiceSettings.ThreadCount).Wait();
                }
                finally
                {
                    if(await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                        await PublishAllEvents();
                }
            }
            return taskList.TotalCount;
        }

        private async System.Threading.Tasks.Task HandleTaskStatusTraking(Domain.Task.Entity.Task task, CancellationTokenSource cancellationTokenSource)
        {
            if (!task.DependencyTaskId.HasValue)
            {
                if (task.TaskStatusId == TaskStatus.New.Id && task.StartDate <= DateTime.Now && task.DueDate >= DateTime.Now)
                {
                    task.TaskStatusId = TaskStatus.InProgress.Id;
                    _events.Add(new TaskInProgressEvent(task));
                }
                else if (task.TaskStatusId == TaskStatus.InProgress.Id && task.DueDate < DateTime.Now)
                {
                    task.TaskStatusId = TaskStatus.InProgressWithDelay.Id;
                }
            }
            else
            {
                Domain.Task.Entity.Task dependencyTask = await _taskRepository.GetTaskById(task.DependencyTaskId.Value);
                if (dependencyTask != null)
                {
                    if (dependencyTask.TaskStatusId == TaskStatus.CompletedEarly.Id ||
                        dependencyTask.TaskStatusId == TaskStatus.CompletedLate.Id ||
                        dependencyTask.TaskStatusId == TaskStatus.CompletedOnTime.Id)
                    {
                        if (task.TaskStatusId == TaskStatus.New.Id && task.StartDate <= DateTime.Now && task.DueDate >= DateTime.Now)
                        {
                            task.TaskStatusId = TaskStatus.InProgress.Id;
                            _events.Add(new TaskInProgressEvent(task));
                        }
                        else if (task.TaskStatusId == TaskStatus.InProgress.Id && task.DueDate < DateTime.Now)
                        {
                            task.TaskStatusId = TaskStatus.InProgressWithDelay.Id;
                        }
                    }
                }
            }
        }

        private async System.Threading.Tasks.Task PublishAllEvents()
        {
            foreach (var _event in _events)
            {
                await _mediator.Publish(_event);
            }
            _events.Clear();
        }


    }
}

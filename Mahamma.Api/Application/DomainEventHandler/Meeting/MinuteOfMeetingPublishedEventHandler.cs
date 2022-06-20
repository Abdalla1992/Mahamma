using Mahamma.Base.Dto.Enum;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.Meeting.Enum;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Event;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Notification.ApiClient.Dto.Notification;
using Mahamma.Notification.ApiClient.Enum;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Api.Application.DomainEventHandler.Meeting
{
    public class MinuteOfMeetingPublishedEventHandler : INotificationHandler<MinuteOfMeetingPublishedEvent>
    {
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public MinuteOfMeetingPublishedEventHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IMediator mediator)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _mediator = mediator;
        }

        public async Task Handle(MinuteOfMeetingPublishedEvent request, CancellationToken cancellationToken)
        {
            List<MinuteOfMeetingActionDto> minuteOfMeetingActionDtoList = new List<MinuteOfMeetingActionDto>();
            foreach (var minuteOfMeeting in request.Meeting.MinutesOfMeeting)
            {
                MinuteOfMeetingActionDto minuteOfMeetingActionDto = new();
                minuteOfMeetingActionDto.ActionLevel = minuteOfMeeting.MinuteOfMeetingLevel;
                if (minuteOfMeeting.MinuteOfMeetingLevel == MinuteOfMeetingLevel.NewProject.Id && minuteOfMeeting.ProjectId != null && minuteOfMeeting.ProjectId > 0)
                {
                    Domain.Project.Entity.Project project = await _projectRepository.GetProject((int)minuteOfMeeting.ProjectId, "ProjectMembers");
                    minuteOfMeetingActionDto.ActionTitle = project.Name;
                    if (project.DeletedStatus == DeletedStatus.Deleted.Id)
                    {
                        project.Activate();
                    }
                }
                else if ((minuteOfMeeting.MinuteOfMeetingLevel == MinuteOfMeetingLevel.NewTask.Id || minuteOfMeeting.MinuteOfMeetingLevel == MinuteOfMeetingLevel.NewSubTask.Id) && minuteOfMeeting.TaskId != null && minuteOfMeeting.TaskId > 0)
                {
                    Domain.Task.Entity.Task task = await _taskRepository.GetTask((int)minuteOfMeeting.TaskId, "TaskMembers");
                    minuteOfMeetingActionDto.ActionTitle = task.Name;
                    if (task.DeletedStatus == DeletedStatus.Deleted.Id)
                    {
                        task.Activate();
                        await TaskAddedEvent(task, request.CreatorUser);
                    }
                }
                else
                {
                    minuteOfMeetingActionDto.ActionTitle = minuteOfMeeting.Description;
                }
                minuteOfMeetingActionDtoList.Add(minuteOfMeetingActionDto);
            }
            await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            await MinutesOfMeetingAddedEvent(request.Meeting.Id, request.Meeting.Title, request.Meeting.Members, minuteOfMeetingActionDtoList, request.CreatorUser);
        }

        private async Task<bool> TaskAddedEvent(Domain.Task.Entity.Task request, UserDto currentUser)
        {
            TaskAddedEvent taskAddedEvent = new(request, currentUser);
            await _mediator.Publish(taskAddedEvent);
            return true;
        }
        private async Task<bool> MinutesOfMeetingAddedEvent(int meetingId, string meetingTitle, List<MeetingMember> members, List<MinuteOfMeetingActionDto> minutesOfMeeting, UserDto currentUser)
        {
            MinuteOfMeetingAddedEvent minuteOfMeetingAddedEvent = new(meetingId, meetingTitle, members, minutesOfMeeting, currentUser);
            await _mediator.Publish(minuteOfMeetingAddedEvent);
            return true;
        }
    }
}

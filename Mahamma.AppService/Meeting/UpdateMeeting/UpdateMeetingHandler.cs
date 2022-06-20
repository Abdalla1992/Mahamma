using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.Meeting.Enum;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Integration.Meeting.Zoom.Dto.Response;
using Mahamma.Integration.Meeting.Zoom.IService;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.UpdateMeeting
{
    public class UpdateMeetingCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int DurationUnitType { get; set; }
        [DataMember]
        public bool IsOnline { get; set; }
        [DataMember]
        public List<MemberMeetingRolesDto> Members { get; set; }
        [DataMember]
        public List<AgendaTopicDto> Agenda { get; set; }
        [DataMember]
        public int? WorkSpaceId { get; set; }
        [DataMember]
        public int? ProjectId { get; set; }
        [DataMember]
        public int? TaskId { get; set; }
        [DataMember]
        public List<MeetingFilesDto> MeetingFiles { get; set; }
        #endregion
    }

    class UpdateMeetingHandler : IRequestHandler<UpdateMeetingCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop

        private readonly IHttpContextAccessor _httpContext;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IMediator _mediator;
        private readonly IMeetingService _zoomMeetingService;

        #endregion

        #region Ctor
        public UpdateMeetingHandler(IHttpContextAccessor httpContext, IMeetingRepository meetingRepository,
            IMessageResourceReader messageResourceReader, IMediator mediator, IMeetingService zoomMeetingService)
        {
            _httpContext = httpContext;
            _meetingRepository = meetingRepository;
            _messageResourceReader = messageResourceReader;
            _mediator = mediator;
            _zoomMeetingService = zoomMeetingService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Meeting.Entity.Meeting meeting = await _meetingRepository.GetById(request.Id);

            #region members creation
            List<MeetingMember> meetingMembers = new();
            foreach (var userId in request.Members)
            {
                MeetingMember meetingMember = new MeetingMember();
                meetingMember.CreateMeetingMember(userId.UserId, meeting.Id, userId.MeetingRoles);
                meetingMembers.Add(meetingMember);
            }
            MeetingMember currentProjectMember = new();
            currentProjectMember.CreateMeetingMember(currentUser.Id, meeting.Id, new List<int> { MeetingRole.Creator.Id }, true, true);
            meetingMembers.Add(currentProjectMember);
            #endregion

            #region agendatopics creation
            List<MeetingAgendaTopics> meetingAgendaTopics = new();
            foreach (AgendaTopicDto agenda in request.Agenda)
            {
                MeetingAgendaTopics meetingAgendaTopic = new();
                meetingAgendaTopic.CreateMeetingTopic(meeting.Id, agenda.Topic, agenda.DurationInMinutes);
                meetingAgendaTopics.Add(meetingAgendaTopic);
            }
            #endregion
            #region Update Meeting files
            List<MeetingFile> meetingFiles = new();
            foreach (MeetingFilesDto file in request?.MeetingFiles)
            {
                MeetingFile meetingFile = new();
                meetingFile.CreateMeetingFile(meeting.Id, file.URL, file.Name);
                meetingFiles.Add(meetingFile);
            } 
            #endregion
            meeting.UpdateMeeting(request.Title, request.Date, request.Date.TimeOfDay, request.Duration, request.DurationUnitType, meetingAgendaTopics, meetingMembers, request.IsOnline,meetingFiles);

            bool isUpdated = await UpdateZoomMeeting(meeting);

            if (isUpdated)
            {
                _meetingRepository.UpdateMeeting(meeting);

                if (await _meetingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    await MeetingUpdatedEvent(meeting, currentUser);
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Added Successfully";
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedTmodify", currentUser.LanguageId);
            }
            return response;
        }

        private async Task<bool> UpdateZoomMeeting(Domain.Meeting.Entity.Meeting meeting)
        {
            bool isUpdated = false;
            MeetingResponseDto meetingResponseDto = _zoomMeetingService.GetById(meeting.ZoomMeetingId);
            if (meetingResponseDto != null)
            {
                meetingResponseDto.Duration = meeting.DurationUnitType == DurationUnitType.Hours.Id ? meeting.Duration * 60 : meeting.Duration;
                meetingResponseDto.topic = meeting.Title;
                isUpdated = await _zoomMeetingService.UpdateMeeting(meeting.ZoomMeetingId, meetingResponseDto);
            }
            return isUpdated;
        }
        private async Task<bool> MeetingUpdatedEvent(Domain.Meeting.Entity.Meeting request, UserDto currentUser)
        {
            MeetingAddedEvent meetingAddedEvent = new(request, currentUser);
            await _mediator.Publish(meetingAddedEvent);
            return true;
        }
    }
}

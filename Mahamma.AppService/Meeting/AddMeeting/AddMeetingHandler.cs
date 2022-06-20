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
using Mahamma.Integration.Meeting.Zoom.Dto;
using Mahamma.Integration.Meeting.Zoom.Dto.Response;
using Mahamma.Integration.Meeting.Zoom.IService;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.AddMeeting
{
    public class AddMeetingCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int DurationUnitType { get; set; }
        [DataMember]
        public int? WorkSpaceId { get; set; }
        [DataMember]
        public int? ProjectId { get; set; }
        [DataMember]
        public int? TaskId { get; set; }
        [DataMember]
        public List<MemberMeetingRolesDto> Members { get; set; }
        [DataMember]
        public List<AgendaTopicDto> Agenda { get; set; }
        [DataMember]
        public bool IsOnline { get; set; }
        [DataMember]
        public List<MeetingFilesDto> MeetingFiles { get; set; }
        #endregion
    }

    class AddMeetingCommandHandler : IRequestHandler<AddMeetingCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IMediator _mediator;
        private readonly IMeetingService _meetingService;
        #endregion

        #region Ctor
        public AddMeetingCommandHandler(IHttpContextAccessor httpContext, IMeetingRepository meetingRepository,
            IMessageResourceReader messageResourceReader, IMediator mediator, IMeetingService meetingService)
        {
            _httpContext = httpContext;
            _meetingRepository = meetingRepository;
            _messageResourceReader = messageResourceReader;
            _mediator = mediator;
            _meetingService = meetingService;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Meeting.Entity.Meeting meeting = new();

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
            foreach (MeetingFilesDto file in request.MeetingFiles)
            {
                MeetingFile meetingFile = new();
                meetingFile.CreateMeetingFile(meeting.Id, file.URL, file.Name);
                meetingFiles.Add(meetingFile);
            }
            #endregion
            meeting.CreateMeeting(request.Title, request.Date, request.Date.TimeOfDay, request.Duration, request.DurationUnitType, currentUser.CompanyId, request.WorkSpaceId, request.ProjectId, request.TaskId
                , currentUser.Id, meetingAgendaTopics, meetingMembers, request.IsOnline,meetingFiles);

            bool isCreated = await CreateZoomMeeting(meeting);

            if (isCreated)
            {

                _meetingRepository.AddMeeting(meeting);

                if (await _meetingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    await MeetingAddedEvent(meeting, currentUser);
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
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
            }
            return response;
        }

        private async Task<bool> CreateZoomMeeting(Domain.Meeting.Entity.Meeting meeting)
        {
            bool isCreated = false;

            MeetingRequestDto meetingRequestDto = new()
            {
                Duration = meeting.DurationUnitType == DurationUnitType.Hours.Id ? meeting.Duration * 60 : meeting.Duration,
                Topic = meeting.Title,
                Type = MeetingTypes.Scheduled,
            };
            MeetingResponseDto meetingResponse = await _meetingService.AddMeeting(meetingRequestDto);
            if (meetingResponse != null)
            {
                isCreated = true;
            }
            meeting.SetZoomMeetingData(meetingResponse.JoinUrl,meetingResponse.MeetingId);
            return isCreated;
        }

        private async Task<bool> MeetingAddedEvent(Domain.Meeting.Entity.Meeting request, UserDto currentUser)
        {
            MeetingAddedEvent meetingAddedEvent = new(request, currentUser);
            await _mediator.Publish(meetingAddedEvent);
            return true;
        }
    }
}

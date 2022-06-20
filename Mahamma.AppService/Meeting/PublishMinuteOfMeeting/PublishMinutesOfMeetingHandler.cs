using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.PublishMinuteOfMeeting
{
    public class PublishMinuteOfMeetingCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int MeetingId { get; set; }
        [DataMember]
        public List<long> Attendees { get; set; }
        #endregion
    }

    class PublishMinutesOfMeetingHandler : IRequestHandler<PublishMinuteOfMeetingCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop

        private readonly IHttpContextAccessor _httpContext;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IMediator _mediator;
        #endregion

        #region Ctor
        public PublishMinutesOfMeetingHandler(IHttpContextAccessor httpContext, IMeetingRepository meetingRepository, IMessageResourceReader messageResourceReader, IMediator mediator)
        {
            _httpContext = httpContext;
            _meetingRepository = meetingRepository;
            _messageResourceReader = messageResourceReader;
            _mediator = mediator;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(PublishMinuteOfMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Meeting.Entity.Meeting meeting = await _meetingRepository.GetById(request.MeetingId);

            meeting.SubmitAttendance(request.Attendees);

            meeting.PublicAllMinutesOfMeeting();

            _meetingRepository.UpdateMeeting(meeting);

            if (await _meetingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                await MintuteOfMeetingPublishedEvent(meeting, currentUser);
                
                response.Result.ResponseData = true;
                response.Result.CommandMessage = "Data Added Successfully";
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
            }
            return response;
        }
        private async Task<bool> MintuteOfMeetingPublishedEvent(Domain.Meeting.Entity.Meeting request, UserDto currentUser)
        {
            MinuteOfMeetingPublishedEvent minuteOfMeetingPublishedEvent = new(request, currentUser);
            await _mediator.Publish(minuteOfMeetingPublishedEvent);
            return true;
        }
    }
}

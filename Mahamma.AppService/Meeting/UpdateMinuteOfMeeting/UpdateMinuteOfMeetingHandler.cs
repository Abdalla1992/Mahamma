using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.UpdateMinuteOfMeeting
{
    public class UpdateMinuteOfMeetingCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int MeetingId { get; set; }
        [DataMember]
        public int MinuteOfMeetingId { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion
    }

    class UpdateMinuteOfMeetingHandler : IRequestHandler<UpdateMinuteOfMeetingCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop

        private readonly IHttpContextAccessor _httpContext;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IMediator _mediator;
        #endregion

        #region Ctor
        public UpdateMinuteOfMeetingHandler(IHttpContextAccessor httpContext, IMeetingRepository meetingRepository, IMessageResourceReader messageResourceReader, IMediator mediator)
        {
            _httpContext = httpContext;
            _meetingRepository = meetingRepository;
            _messageResourceReader = messageResourceReader;
            _mediator = mediator;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateMinuteOfMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Meeting.Entity.Meeting meeting = await _meetingRepository.GetById(request.MeetingId);

            var minuteOfMeeting = meeting.MinutesOfMeeting.FirstOrDefault(m => m.Id == request.MinuteOfMeetingId);
            minuteOfMeeting.UpdateMinuteOfMeeting(request.Description);

            _meetingRepository.UpdateMeeting(meeting);

            if (await _meetingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = "Data Added Successfully";
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
            }
            return response;
        }
    }
}

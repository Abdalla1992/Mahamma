using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.DeleteMinuteOfMeeting
{

    public class DeleteMinuteOfMeetingCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int MeetingId { get; set; }
        [DataMember]
        public int MinuteofMeetingId { get; set; }
        #endregion
        public DeleteMinuteOfMeetingCommand(int id, int meetingId)
        {
            MinuteofMeetingId = id;
            MeetingId = meetingId;
        }
    }

    public class DeleteMinuteOfMeetingHandler : IRequestHandler<DeleteMinuteOfMeetingCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMediator _mediator;
        #endregion
        public DeleteMinuteOfMeetingHandler(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, IMeetingRepository meetingRepository, IMediator mediator)
        {
            _httpContext = httpContext;
            _meetingRepository = meetingRepository;
            _mediator = mediator;
        }

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteMinuteOfMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Mahamma.Domain.Meeting.Entity.Meeting meeting = await _meetingRepository.GetById(request.MeetingId);
            if (meeting != null)
            {
                var minuteOfMeeting = meeting.MinutesOfMeeting.FirstOrDefault(t => t.Id == request.MinuteofMeetingId);
                if (minuteOfMeeting != null)
                {
                    minuteOfMeeting.DeleteMinuteOfMeeting();
                }

                _meetingRepository.UpdateMeeting(meeting);
                if (await _meetingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Deleted Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed To Delete";
                }
            }
            else
            {
                response.Result.CommandMessage = "No data Found";
            }
            return response;
        }
    }
}

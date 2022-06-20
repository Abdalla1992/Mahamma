using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Integration.Meeting.Zoom.IService;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.CancelMeeting
{
    public class CancelMeetingCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int MeetingId { get; set; }
        #endregion
        public CancelMeetingCommand(int id)
        {
            MeetingId = id;
        }
    }

    class CancelMeetingHandler : IRequestHandler<CancelMeetingCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop

        private readonly IHttpContextAccessor _httpContext;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IMediator _mediator;
        private readonly IMeetingService _zoomMeetingService;

        #endregion

        #region Ctor
        public CancelMeetingHandler(IHttpContextAccessor httpContext, IMeetingRepository meetingRepository,
            IMessageResourceReader messageResourceReader, IMediator mediator, IMeetingService zoomMeetingService)
        {
            _httpContext = httpContext;
            _meetingRepository = meetingRepository;
            _messageResourceReader = messageResourceReader;
            _mediator = mediator;
            _zoomMeetingService = zoomMeetingService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(CancelMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Meeting.Entity.Meeting meeting = await _meetingRepository.GetById(request.MeetingId);

            bool isDelete = await DeleteZoomMeeting(meeting);
            if (isDelete)
            {
                meeting.DeleteMeeting(currentUser.Id);
                if (await _meetingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    if (meeting.Date > DateTime.Now)
                    {
                        await MeetingCanceledEvent(meeting, currentUser);
                    }
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Deleted Successfully";
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage = "Failed To Delete";
            }
            return response;
        }

        private async Task<bool> DeleteZoomMeeting(Domain.Meeting.Entity.Meeting meeting)
        {
            bool isDeleted = false;
            bool zoomDeleted = await _zoomMeetingService.DeleteMeeting(meeting.ZoomMeetingId);
            if (zoomDeleted)
            {
                isDeleted = true;
            }
            return isDeleted;
        }

        private async Task<bool> MeetingCanceledEvent(Domain.Meeting.Entity.Meeting request, UserDto currentUser)
        {
            MeetingCanceledEvent meetingAddedEvent = new(request, currentUser);
            await _mediator.Publish(meetingAddedEvent);
            return true;
        }
    }
}

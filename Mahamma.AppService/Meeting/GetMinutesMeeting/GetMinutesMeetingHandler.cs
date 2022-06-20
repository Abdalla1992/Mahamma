using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.GetMinutesMeeting
{
    public class GetMinutesMeetingCommand : IRequest<ValidateableResponse<ApiResponse<List<MinuteOfMeetingActionDto>>>>
    {
        #region Prop
        [DataMember]
        public int MeetingId { get; set; }
        #endregion
        public GetMinutesMeetingCommand(int id)
        {
            MeetingId = id;
        }
    }

    class GetMinutesMeetingCommandHandler : IRequestHandler<GetMinutesMeetingCommand, ValidateableResponse<ApiResponse<List<MinuteOfMeetingActionDto>>>>
    {
        #region Prop

        private readonly IHttpContextAccessor _httpContext;
        private readonly IMeetingReadRepository _meetingReadRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IAccountService _accountService;
        #endregion

        #region Ctor
        public GetMinutesMeetingCommandHandler(IHttpContextAccessor httpContext, IMessageResourceReader messageResourceReader, IMeetingReadRepository meetingReadRepository, IAccountService accountService)
        {
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            _meetingReadRepository = meetingReadRepository;
            _accountService = accountService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<List<MinuteOfMeetingActionDto>>>> Handle(GetMinutesMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<List<MinuteOfMeetingActionDto>>> response = new(new ApiResponse<List<MinuteOfMeetingActionDto>>());
            List<MinuteOfMeetingActionDto> minutesOfmeeting = await _meetingReadRepository.GetMinutesOfMeeting(request.MeetingId, currentUser.Id);
            #region Get meeting members
            foreach (var minuteOfmeeting in minutesOfmeeting)
            {
                List<long> membersIds = minuteOfmeeting.Assignee.Split(',').Select(a => long.Parse(a)).ToList();
                if (membersIds?.Count > 0)
                {
                    minuteOfmeeting.Members = new();
                    foreach (var memberId in membersIds)
                    {
                        UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, memberId);
                        if (userDto != null)
                        {
                            minuteOfmeeting.Members.Add(new Domain.MemberSearch.Dto.MemberDto
                            {
                                UserId = userDto.Id,
                                FullName = userDto.FullName,
                                ProfileImage = userDto.ProfileImage
                            });
                        }
                    }
                }
            }
            #endregion
            if (minutesOfmeeting != null && minutesOfmeeting.Count > 0)
            {
                response.Result.ResponseData = minutesOfmeeting;
                response.Result.CommandMessage = "Data exists";
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
            }
            return response;
        }

        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }
    }
}

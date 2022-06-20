using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Entity;
using Mahamma.Domain.Meeting.Event;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.GetMeeting
{
    public class GetMeetingCommand : IRequest<ValidateableResponse<ApiResponse<MeetingDto>>>
    {
        #region Prop
        [DataMember]
        public int MeetingId { get; set; }
        #endregion
        public GetMeetingCommand(int id)
        {
            MeetingId = id;
        }
    }

    class GetMeetingCommandHandler : IRequestHandler<GetMeetingCommand, ValidateableResponse<ApiResponse<MeetingDto>>>
    {
        #region Prop

        private readonly IHttpContextAccessor _httpContext;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IAccountService _accountService;
        private readonly AppSetting _appSetting;
        #endregion

        #region Ctor
        public GetMeetingCommandHandler(IHttpContextAccessor httpContext, IMeetingRepository meetingRepository, IMessageResourceReader messageResourceReader, AppSetting appSetting, IAccountService accountService)
        {
            _httpContext = httpContext;
            _meetingRepository = meetingRepository;
            _messageResourceReader = messageResourceReader;
            _appSetting = appSetting;
            _accountService = accountService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<MeetingDto>>> Handle(GetMeetingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<MeetingDto>> response = new(new ApiResponse<MeetingDto>());
            MeetingDto meeting = await _meetingRepository.GetDtoById(request.MeetingId, currentUser.Id, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.CompanyId);
            #region Get meeting members
            if (meeting != null)
                foreach (var member in meeting.MemberList)
                {
                    UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, member.Key);
                    if (userDto != null)
                    {
                        meeting.Members.Add(new Domain.MemberSearch.Dto.MemberDto
                        {
                            UserId = userDto.Id,
                            FullName = userDto.FullName,
                            ProfileImage = userDto.ProfileImage
                        });
                    }
                    if (meeting.CreatorUserId != currentUser.Id)
                        meeting.MeetingFiles = new();
                }
            #endregion
            if (meeting != null)
            {
                response.Result.ResponseData = meeting;
                response.Result.CommandMessage = "Data Added Successfully";
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

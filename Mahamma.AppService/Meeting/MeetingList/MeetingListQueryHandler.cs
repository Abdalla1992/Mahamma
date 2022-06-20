using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.MeetingList
{
    public class MeetingListQueryHandler : IRequestHandler<SearchMeetingDto, PageList<MeetingDto>>
    {
        #region Props
        private readonly IMeetingRepository _meetingRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        private readonly IAccountService _accountService;
        #endregion

        #region CTRS
        public MeetingListQueryHandler(IMeetingRepository meetingRepository, IHttpContextAccessor httpContext, AppSetting appSetting, IAccountService accountService)
        {
            _meetingRepository = meetingRepository;
            _httpContext = httpContext;
            _appSetting = appSetting;
            _accountService = accountService;
        }
        #endregion

        public async Task<PageList<MeetingDto>> Handle(SearchMeetingDto request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            PageList<MeetingDto> meetingList = await _meetingRepository.GetMeetings(request, currentUser.RoleName, _appSetting.SuperAdminRole, currentUser.Id, currentUser.CompanyId);
            #region Get meeting members
            foreach (var meeting in meetingList.DataList)
            {
                IDictionary<long, List<int>> members = meeting.MemberList;//await _meetingRepository.GetMembersUserIdList(meeting.Id);
                if (members?.Count > 0)
                {
                    foreach (var member in members)
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
                    }
                }
                if (meeting.CreatorUserId != currentUser.Id)
                    meeting.MeetingFiles = new();
            }
            #endregion
            return meetingList;
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

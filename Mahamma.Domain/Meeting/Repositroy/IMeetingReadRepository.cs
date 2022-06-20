using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Meeting.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Meeting.Repositroy
{
    public interface IMeetingReadRepository
    {
        Task<List<MinuteOfMeetingActionDto>> GetMinutesOfMeeting(int meetingId, long id);
    }
}

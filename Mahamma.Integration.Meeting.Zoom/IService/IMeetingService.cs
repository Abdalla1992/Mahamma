using Mahamma.Integration.Meeting.Zoom.Dto;
using Mahamma.Integration.Meeting.Zoom.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Integration.Meeting.Zoom.IService
{
    public interface IMeetingService
    {
        Task<MeetingResponseDto> AddMeeting(MeetingRequestDto meetingDto);
         MeetingResponseDto GetById(long meetingId);
        Task<bool> UpdateMeeting(long meetingId , MeetingResponseDto meetingDto);
        Task<bool> DeleteMeeting(long meetingId);
        Task<List<MeetingResponseDto>> GetAll(string userId);

    }
}

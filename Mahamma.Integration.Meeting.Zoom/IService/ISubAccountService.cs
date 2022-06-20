using Mahamma.Integration.Meeting.Zoom.Dto.Request;
using Mahamma.Integration.Meeting.Zoom.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Integration.Meeting.Zoom.IService
{
    public interface ISubAccountService
    {
        Task<SubAccountResponseDto> AddMeeting(SubAccountRequestDto subAccountDto);
        Task<bool> DeleteSubAccount(long subAccountId);
        SubAccountResponseDto GetById(long meetingId);




    }
}

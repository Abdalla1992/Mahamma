using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Request;
using MediatR;

namespace Mahamma.Domain.Meeting.Dto
{
    public class SearchMeetingDto : SearchDto<MeetingDto>, IRequest<PageList<MeetingDto>>
    {
    }
}

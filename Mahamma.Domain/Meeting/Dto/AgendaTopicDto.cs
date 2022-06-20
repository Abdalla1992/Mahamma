using Mahamma.Base.Dto.Dto;

namespace Mahamma.Domain.Meeting.Dto
{
    public class AgendaTopicDto : BaseDto<int>
    {
        public string Topic { get; set; }
        public int DurationInMinutes { get; set; }
    }
}

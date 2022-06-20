using Mahamma.Base.Dto.Dto;
namespace Mahamma.Domain.Task.Dto
{
    public class TaskMemberDto : BaseDto<int>
    {
        public long UserId { get; set; }
        public int TaskId { get; set; }
        public double? Rating { get; set; }
        public string FullName { get; set; }
        public int TaskAcceptedRejectedStatus { get; set; }
    }
}

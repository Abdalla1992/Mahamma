using Mahamma.Base.Dto.Dto;

namespace Mahamma.Domain.Company.Dto
{
    public class CompanyInvitationDto : BaseDto<int>
    {
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string InvitationId { get; set; }
        public int InvitationStatusId { get; set; }
        public string InvitationLink { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string EmployeeType { get; set; }
    }
}

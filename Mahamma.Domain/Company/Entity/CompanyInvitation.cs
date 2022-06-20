using Mahamma.Base.Domain;
using System;

namespace Mahamma.Domain.Company.Entity
{
    public class CompanyInvitation : Entity<int>, IAggregateRoot
    {
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string InvitationId { get; set; }
        public int InvitationStatusId { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string EmployeeType { get; set; }

        public void CreateCompanyInvitations(int companyId, string email, long userId, string invitationId, int invitationStatusId,
                                                string jobTitle, string role, string employeeType)
        {
            CompanyId = companyId;
            Email = email;
            UserId = userId;
            InvitationId = invitationId;
            InvitationStatusId = invitationStatusId;
            JobTitle = jobTitle;
            Role = role;
            EmployeeType = employeeType;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void UpdateInvitationStatus(int invitationStatusId)
        {
            InvitationStatusId = invitationStatusId;
        }
    }
}

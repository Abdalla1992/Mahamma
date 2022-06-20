using Mahamma.Base.Domain;
using Mahamma.Domain.Company.Enum;
using System;

namespace Mahamma.Domain.Company.Entity
{
    public class CompanyInvitationFile : Entity<int>, IAggregateRoot
    {
        public string FileName { get; set; }
        public int CompanyId { get; set; }
        public long UserId { get; set; }
        public InvitationFileStatus Status { get; set; }

        public void CreateInvitationFile(string fileName, int companyId, long userId, InvitationFileStatus status)
        {
            FileName = fileName;
            CompanyId = companyId;
            UserId = userId;
            Status = status;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void UpdateInvitationStatus(InvitationFileStatus status)
        {
            Status = status;
        }
    }
}

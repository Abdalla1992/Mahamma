using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Enum;

namespace Mahamma.Domain.Company.Repositroy
{
    public interface ICompanyInvitationFileRepository : IRepository<CompanyInvitationFile>
    {
        void AddCompanyInvitationFile(CompanyInvitationFile companyInvitationFile);
        void UpdateCompanyInvitationFileStatus(int fileId, InvitationFileStatus status);

    }
}

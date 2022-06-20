using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Enum;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Infrastructure.Context;
using System.Linq;

namespace Mahamma.Infrastructure.Repositories
{
    public class CompanyInvitationFileRepository : Base.EntityRepository<CompanyInvitationFile>, ICompanyInvitationFileRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;

        public CompanyInvitationFileRepository(MahammaContext appDbContext, IMapper mapper) : base(appDbContext, mapper)
        {
        }

        public void AddCompanyInvitationFile(CompanyInvitationFile companyInvitationFile)
        {
            CreateAsyn(companyInvitationFile);
        }

        public void UpdateCompanyInvitationFileStatus(int fileId, InvitationFileStatus status)
        {
            CompanyInvitationFile entity = AppDbContext.CompanyInvitationFile.SingleOrDefault(f => f.Id == fileId);
            entity.Status = status;
        }
    }
}

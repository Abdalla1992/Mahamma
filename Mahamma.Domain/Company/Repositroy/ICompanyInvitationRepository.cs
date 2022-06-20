using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Company.Repositroy
{
    public interface ICompanyInvitationRepository : IRepository<Entity.CompanyInvitation>
    {
        Task<PageList<Entity.CompanyInvitation>> GetInvitationList(int skipCount, int takeCount, Expression<Func<Entity.CompanyInvitation, bool>> filter = null);
        void UpdateCompanyInvitation(Entity.CompanyInvitation companyInvitation);
        Task<Dto.CompanyInvitationDto> GetCompanyInvitationByInvitationId(string invitationId);
        Task<Entity.CompanyInvitation> GetEntityById(int id);
        Task<Entity.CompanyInvitation> GetCompanyInvitationEntityByInvitationId(string invitationId);
        void AddCompanyInvitation(Entity.CompanyInvitation companyInvitation);
        Dto.CompanyInvitationDto MapCompanyInvitationToCompanyInvitationdto(Entity.CompanyInvitation companyInvitation);
    }
}

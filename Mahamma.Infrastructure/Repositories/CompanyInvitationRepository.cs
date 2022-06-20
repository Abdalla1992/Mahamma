using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class CompanyInvitationRepository : Base.EntityRepository<CompanyInvitation>, ICompanyInvitationRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public CompanyInvitationRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task<PageList<CompanyInvitation>> GetInvitationList(int skipCount, int takeCount, Expression<Func<CompanyInvitation, bool>> filter = null)
        {
            PageList<CompanyInvitation> invitationList = new();
            var remainingInvitationList = GetWhere(filter).Skip(skipCount);
            var Count = await remainingInvitationList.CountAsync();
            var resultList = remainingInvitationList.Take(takeCount);
            invitationList.SetResult(Count, resultList.ToList());
            return invitationList;
        }
        public void UpdateCompanyInvitation(CompanyInvitation companyInvitation)
        {
            Update(companyInvitation);
        }
        public async Task<CompanyInvitationDto> GetCompanyInvitationByInvitationId(string invitationId)
        {
            return Mapper.Map<CompanyInvitationDto>(await FirstOrDefaultAsync(c => c.InvitationId.Equals(invitationId)));
        }
        public async Task<CompanyInvitation> GetCompanyInvitationEntityByInvitationId(string invitationId)
        {
            return await FirstOrDefaultAsync(c => c.InvitationId.Equals(invitationId));
        }
        public void AddCompanyInvitation(CompanyInvitation companyInvitation)
        {
            CreateAsyn(companyInvitation);
        }
        public CompanyInvitationDto MapCompanyInvitationToCompanyInvitationdto(CompanyInvitation companyInvitation)
        {
            return Mapper.Map<CompanyInvitationDto>(companyInvitation);
        }
        public async Task<CompanyInvitation> GetEntityById(int id)
        {
            return await FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}

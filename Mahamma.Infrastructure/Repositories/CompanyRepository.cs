using AutoMapper;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class CompanyRepository : Base.EntityRepository<Company>, ICompanyRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public CompanyRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public void AddCompany(Company company)
        {
            CreateAsyn(company);
        }

        public Task<bool> CheckCompanyExistence(string name, int id)
        {
            return GetAnyAsync(w => w.Name == name && (id == default || w.Id != id) && w.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }

        public async Task<CompanyDto> GetCompanyData(int id)
        {
            Company company = await FirstOrDefaultAsync(t => t.Id == id);
            return Mapper.Map<CompanyDto>(company);
        }
    }
}

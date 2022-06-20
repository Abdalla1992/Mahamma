using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Project.Dto;
using System;
using System.Threading.Tasks;

namespace Mahamma.Domain.Company.Repositroy
{
    public interface ICompanyRepository : IRepository<Entity.Company>
    {
        void AddCompany(Entity.Company company);
        Task<bool> CheckCompanyExistence(string name, int id);
        Task<CompanyDto> GetCompanyData(int id);
    }
}

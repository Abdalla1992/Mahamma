using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Company.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Company.Repositroy
{
    public interface ICompanyReadRepository 
    {
        Task<PageList<CompanyDetailsDto>> GetCompanyWithWorkspaces(SearchCompanyDetailsDto searchCompanyDetailsDto, int companyId, long currentUserId,
           string role,string superAdminRole);
    }
}

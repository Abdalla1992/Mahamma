using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Infrastructure.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class CompanyReadRepository : DapperRepository, ICompanyReadRepository
    {
        #region Ctor
        public CompanyReadRepository(IConfiguration config) : base(config)
        {
        }
        #endregion



        public async Task<PageList<CompanyDetailsDto>> GetCompanyWithWorkspaces(SearchCompanyDetailsDto searchCompanyDetailsDto, int companyId, long currentUserId,
            string role, string superAdminRole)
        {
            PageList<CompanyDetailsDto> companyDetailsDto = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@name", searchCompanyDetailsDto.Filter.WorkspaceName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            queryParams.Add("@companyId", companyId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@currentUserId", currentUserId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@role", role, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@superAdminRole", superAdminRole, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@pageSize", searchCompanyDetailsDto.Paginator.PageSize, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@pageIndex", searchCompanyDetailsDto.Paginator.Page, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@sortColumn", searchCompanyDetailsDto.Sorting.Column, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            queryParams.Add("@sortDirection", searchCompanyDetailsDto.Sorting.SortingDirection.Id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@totalCount", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);


            List<CompanyDetailsDto> result = (await GetListAsync<CompanyDetailsDto>("dbo.sp_GetCompanyWithWorkspaces", queryParams, System.Data.CommandType.StoredProcedure)).ToList();
            if (result?.Count > default(int))
            {
                int totalCounts = queryParams.Get<int>("@totalCount");
                companyDetailsDto.SetResult(totalCounts, result);
            }
            return companyDetailsDto;
        }
    }
}

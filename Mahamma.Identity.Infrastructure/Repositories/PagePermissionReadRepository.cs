using Mahamma.Identity.Domain.PagePermissionByRoleId.Dto;
using Mahamma.Identity.Domain.PagePermissionByRoleId.Repository;
using Mahamma.Identity.Infrastructure.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class PagePermissionReadRepository : DapperRepository, IPagePermissionLocalizationRepository
    {
        public PagePermissionReadRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<List<PagePermissionLocalizationDto>> GetPagePermissionByRoleId(long currentUserId, long currentRoleId)
        {
            List<PagePermissionLocalizationDto> result = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@currentUserId", currentUserId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@currentRoleId", currentRoleId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            result = (await GetListAsync<PagePermissionLocalizationDto>("sp_GetPageAndPermissionBasedInRoles", queryParams, System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }
    }
}

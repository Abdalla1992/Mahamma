using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Repository;
using System;
using System.Threading.Tasks;
using Mahamma.Domain.Task.Dto;
using Mahamma.Infrastructure.Base;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Mahamma.Domain.MemberSearch.Dto;
using System.Linq;

namespace Mahamma.Infrastructure.Repositories
{
    public class MemberSearchReadRepository : DapperRepository, IMemberSearchReadRepository
    {
        public MemberSearchReadRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<List<MemberDto>> SearchForMemerToAssignToWorkspace(string name, int companyId, long currentUserId, int workspaceId)
        {
            List<MemberDto> result = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@Name", name, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            queryParams.Add("@companyId", companyId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@workspaceId", workspaceId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@currentUserId", currentUserId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            result = (await GetListAsync<MemberDto>("dbo.sp_SearchInUserForAssigningToWorkspace", queryParams, System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public async Task<List<MemberDto>> SearchForMemerToAssignToProject(string name, int companyId, long currentUserId, int projectId)
        {
            List<MemberDto> result = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@Name", name, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            queryParams.Add("@companyId", companyId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@projectId", projectId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@currentUserId", currentUserId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            result = (await GetListAsync<MemberDto>("dbo.sp_SearchInUserForAssigningToProject", queryParams, System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public async Task<List<MemberDto>> SearchForMemerToAssignToTask(string name, int companyId, long currentUserId, int taskId)
        {
            List<MemberDto> result = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@Name", name, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            queryParams.Add("@companyId", companyId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@taskId", taskId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            queryParams.Add("@currentUserId", currentUserId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            result = (await GetListAsync<MemberDto>("dbo.sp_SearchInUserForAssigningToTask", queryParams, System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }
    }
}

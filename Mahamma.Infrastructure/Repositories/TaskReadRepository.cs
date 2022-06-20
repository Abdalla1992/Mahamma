using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Repository;
using System;
using System.Threading.Tasks;
using Mahamma.Domain.Task.Dto;
using Mahamma.Infrastructure.Base;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Mahamma.Infrastructure.Repositories
{
    public class TaskReadRepository : DapperRepository, ITaskReadRepository
    {
        public TaskReadRepository(IConfiguration config) : base(config)
        {
        }

        public async Task<TaskDto> GetTaskData(int TaskId)
        {
            TaskDto result = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@TaskId", TaskId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            var sp_result = await GetAsync<TaskDto>("dbo.SP_GetTaskDetails", queryParams, System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<List<UserTaskDto>> GetUserTask(long userId)
        {
            List<UserTaskDto> result = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@userId", userId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input);
            var sp_result = await GetListAsync<UserTaskDto>("dbo.spGetUserTasks", queryParams, System.Data.CommandType.StoredProcedure);
            result = sp_result.ToList();
            return result;
        }

        public async Task<List<UserTaskAcceptedRejectedDto>> GetUserTaskAcceptedRejectedStatus(long userId)
        {
            List<UserTaskAcceptedRejectedDto> result = new();
            var QueryParams = new Dapper.DynamicParameters();
            QueryParams.Add("@userId", userId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            var sp_result =await GetListAsync<UserTaskAcceptedRejectedDto>("spGetUserTaskAcceptedRejectedStatus", QueryParams, System.Data.CommandType.StoredProcedure);
            result = sp_result.ToList();
            return result;
        }
    }
}

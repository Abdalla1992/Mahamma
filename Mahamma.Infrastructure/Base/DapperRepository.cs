using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Base
{
    public class DapperRepository
    {
        #region CTRS
        public IConfiguration Configuration { get; }

        public DapperRepository(IConfiguration config)
        {
            Configuration = config;
        }

        public void Dispose()
        {

        }
        #endregion

        #region Connection String
        private string GetConnection()
        {
            string connString = Configuration["ConnectionString"];
            string envConnString = Environment.GetEnvironmentVariable("ConnectionStrings__DBConString");
            if (!string.IsNullOrWhiteSpace(envConnString))
            {
                connString = envConnString;
            }
            Log.Information($"Dapper will run with connection string: {connString}");

            return connString;
        }
        #endregion

        public async Task<IEnumerable<T>> GetListAsync<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(GetConnection());
            return await db.QueryAsync<T>(sp, parms, commandType: commandType);
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(GetConnection());
            return (await db.QueryAsync<T>(sp, parms, commandType: commandType)).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(GetConnection());
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(GetConnection());
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }
    }
}

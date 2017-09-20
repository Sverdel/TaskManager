using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;
using Dapper;

namespace TaskManager.Core.Api.Repository
{
    public class PriorityRepository : IRepository<Priority, int>
    {
        private readonly string _connectionString;
        public PriorityRepository(IConfiguration config)
        {
            _connectionString = config["Data:DefaultConnection:ConnectionString"];
        }

        public async Task<IEnumerable<Priority>> Get()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Priority>("SELECT * FROM dbo.Priorities").ConfigureAwait(false);
            }
        }

        public async Task<Priority> Get(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<Priority>("SELECT * FROM dbo.Priorities where Id = @id", new { id }).ConfigureAwait(false);
            }
        }

        public async Task<Priority> Create(Priority task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string query = "Insert into dbo.Priorities (Name) output inserted.* VALUES(@Name)";
                return await db.QueryFirstOrDefaultAsync<Priority>(query, task).ConfigureAwait(false);
            }
        }

        public async Task Update(Priority task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sqlQuery = "UPDATE dbo.Priorities SET Name = @Name WHERE Id = @Id";
                await db.ExecuteAsync(sqlQuery, task).ConfigureAwait(false);
            }
        }

        public async Task Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sqlQuery = "DELETE FROM dbo.Priorities WHERE Id = @id";
                await db.ExecuteAsync(sqlQuery, new { id }).ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<Priority>> Get(string userId)
        {
            return Get();
        }
    }
}
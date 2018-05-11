using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using TaskManager.Core.Model;

namespace TaskManager.Core.Repository
{
    public class StateRepository : IRepository<State, int>
    {
        private readonly string _connectionString;
        public StateRepository(string connectioString)
        {
            _connectionString = connectioString;
        }

        public async Task<IEnumerable<State>> Get()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<State>("SELECT * FROM dbo.States").ConfigureAwait(false);
            }
        }

        public async Task<State> Get(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<State>("SELECT * FROM dbo.States where Id = @id", new { id }).ConfigureAwait(false);
            }
        }

        public async Task<State> Create(State task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string query = "Insert into dbo.States (Name) output inserted.* VALUES (@Name)";
                return await db.QueryFirstOrDefaultAsync<State>(query, task).ConfigureAwait(false);
            }
        }

        public async Task<State> Update(State task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string query = "UPDATE dbo.States SET Name = @Name output inserted.* WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<State>(query, task).ConfigureAwait(false);
            }
        }

        public async Task Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sqlQuery = "DELETE FROM dbo.States WHERE Id = @id";
                await db.ExecuteAsync(sqlQuery, new { id }).ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<State>> Get(string userId)
        {
            return Get();
        }
    }
}
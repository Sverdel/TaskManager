using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;
using Dapper;

namespace TaskManager.Core.Api.Repository
{
    public class StateRepository : IRepository<State, int>
    {
        private string _connectionString;
        public StateRepository(IConfiguration config)
        {
            _connectionString = config["Data:DefaultConnection:ConnectionString"];
        }

        public async Task<IEnumerable<State>> Get()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<State>("SELECT * FROM dbo.States");
            }
        }

        public async Task<State> Get(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<State>("SELECT * FROM dbo.States where Id = @id", new { id });
            }
        }

        public async Task Create(State task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "Insert into dbo.States (Name) VALUES (@Name)";
                await db.ExecuteAsync(query, task);
            }
        }

        public async Task Update(State task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE dbo.States SET Name = @Name WHERE Id = @Id";
                await db.ExecuteAsync(sqlQuery, task);
            }
        }

        public async Task Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM dbo.States WHERE Id = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }

    }
}
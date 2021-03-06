﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using TaskManager.Core.Model;

namespace TaskManager.Core.Repository
{
    public class PriorityRepository : IRepository<Priority, int>
    {
        private readonly string _connectionString;
        public PriorityRepository(string connectioString)
        {
            _connectionString = connectioString;
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

        public async Task<Priority> Update(Priority task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string query = "UPDATE dbo.Priorities SET Name = @Name output inserted.*  WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<Priority>(query, task).ConfigureAwait(false);
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
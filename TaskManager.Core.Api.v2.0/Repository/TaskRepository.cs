using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;
using Dapper;

namespace TaskManager.Core.Api.Repository
{
    public class TaskRepository : IRepository<WorkTask, long>
    {
        private readonly string _connectionString;
        public TaskRepository(IConfiguration config)
        {
            _connectionString = config["Data:DefaultConnection:ConnectionString"];
        }

        public async Task<IEnumerable<WorkTask>> Get()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<WorkTask>("SELECT * FROM dbo.WorkTasks").ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<WorkTask>> Get(string userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<WorkTask>("SELECT * FROM dbo.WorkTasks where userId = @userId", new { userId }).ConfigureAwait(false);
            }
        }

        public async Task<WorkTask> Get(long id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<WorkTask>("SELECT * FROM dbo.WorkTasks where Id = @id", new { id }).ConfigureAwait(false);
            }
        }

        public async Task<WorkTask> Create(WorkTask task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string query = @"Insert into dbo.WorkTasks 
(ActualTimeCost, ChangeDatetime, CreateDateTime, Description, Name, PlanedTimeCost,  PriorityId,  RemainingTimeCost, StateId, UserId, Version)
output inserted.*
VALUES (@ActualTimeCost, current_timestamp, current_timestamp, @Description, @Name, @PlanedTimeCost,  @PriorityId,  @RemainingTimeCost, @StateId, @UserId, @Version)";
                return await db.QueryFirstOrDefaultAsync<WorkTask>(query, task).ConfigureAwait(false);
            }
        }

        public async Task<WorkTask> Update(WorkTask task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string query = @"UPDATE dbo.WorkTasks 
SET ActualTimeCost = @ActualTimeCost, ChangeDatetime = current_timestamp, CreateDateTime = @CreateDateTime, Description = @Description, 
Name = @Name, PlanedTimeCost = @PlanedTimeCost, PriorityId = @PriorityId, RemainingTimeCost = @RemainingTimeCost, StateId = @StateId, 
UserId = @UserId, Version = @Version
output inserted.* 
WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<WorkTask>(query, task).ConfigureAwait(false);
            }
        }

        public async Task Delete(long id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string sqlQuery = "DELETE FROM dbo.WorkTasks WHERE Id = @id";
                await db.ExecuteAsync(sqlQuery, new { id }).ConfigureAwait(false);
            }
        }
    }
}
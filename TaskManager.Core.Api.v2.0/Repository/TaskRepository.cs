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
        private string _connectionString;
        public TaskRepository(IConfiguration config)
        {
            _connectionString = config["Data:DefaultConnection:ConnectionString"];
        }

        public async Task<IEnumerable<WorkTask>> Get()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<WorkTask>("SELECT * FROM dbo.WorkTasks");
            }
        }

        public async Task<IEnumerable<WorkTask>> Get(string userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<WorkTask>("SELECT * FROM dbo.WorkTasks where userId = @userId", new { userId });
            }
        }

        public async Task<WorkTask> Get(long id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<WorkTask>("SELECT * FROM dbo.WorkTasks where Id = @id", new { id });
            }
        }

        public async Task<WorkTask> Create(WorkTask task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = @"Insert into dbo.WorkTasks 
(ActualTimeCost, ChangeDatetime, CreateDateTime, Description, Name, PlanedTimeCost,  PriorityId,  RemainingTimeCost, StateId, UserId, Version)
output inserted.*
VALUES (@ActualTimeCost, @ChangeDatetime, @CreateDateTime, @Description, @Name, @PlanedTimeCost,  @PriorityId,  @RemainingTimeCost, @StateId, @UserId, @Version)";
                return await db.QueryFirstOrDefaultAsync<WorkTask>(query, task);
            }
        }

        public async Task Update(WorkTask task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = @"UPDATE dbo.WorkTasks 
SET ActualTimeCost = @ActualTimeCost, ChangeDatetime = @ChangeDatetime, CreateDateTime = @CreateDateTime, Description = @Description, 
Name = @Name, PlanedTimeCost = @PlanedTimeCost, PriorityId = @PriorityId, RemainingTimeCost = @RemainingTimeCost, StateId = @StateId, 
UserId = @UserId, Version = @Version
WHERE Id = @Id";
                await db.ExecuteAsync(sqlQuery, task);
            }
        }

        public async Task Delete(long id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM dbo.WorkTasks WHERE Id = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }
    }
}
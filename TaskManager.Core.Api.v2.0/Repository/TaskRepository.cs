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
    public class TaskRepository : ITaskRepository
    {
        private string _connectionString;
        public TaskRepository(IConfiguration config)
        {
            _connectionString = config["Data:DefaultConnection:ConnectionString"];
        }

        public async Task<IEnumerable<WorkTask>> GetTasks()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<WorkTask>("SELECT * FROM dbo.WorkTasks");
            }
        }

        public async Task<WorkTask> GetTask(long id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<WorkTask>("SELECT * FROM dbo.WorkTasks where Id = @id", new { id });
            }
        }

        public async Task CreateTask(WorkTask task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = @"Insert into dbo.WorkTasks 
(ActualTimeCost, ChangeDatetime, CreateDateTime, Description, Name, PlanedTimeCost,  PriorityId,  RemainingTimeCost, StateId, UserId, Version)
VALUES (@ActualTimeCost, @ChangeDatetime, @CreateDateTime, @Description, @Name, @PlanedTimeCost,  @PriorityId,  @RemainingTimeCost, @StateId, @UserId, @Version)";
                await db.ExecuteAsync(query, task);
            }
        }

        public async Task UpdateTask(WorkTask task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = @"UPDATE dbo.WorkTasks SET Name = @Name, Age = @Age
SET ActualTimeCost = @ActualTimeCost, ChangeDatetime = @ChangeDatetime, CreateDateTime = @CreateDateTime, Description = @Description, 
Name = @Name, PlanedTimeCost = @PlanedTimeCost, PriorityId = @PriorityId, RemainingTimeCost = @RemainingTimeCost, StateId = @StateId, 
UserId = @UserId, Version = @Version
WHERE Id = @Id";
                await db.ExecuteAsync(sqlQuery, task);
            }
        }

        public async Task DeleteTask(long id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM dbo.WorkTasks WHERE Id = @id";
                await db.ExecuteAsync(sqlQuery, new { id });
            }
        }
    }
}
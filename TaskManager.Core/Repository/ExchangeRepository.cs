using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using TaskManager.Core.Model;

namespace TaskManager.Core.Repository
{
    public class ExchangeRepository : IExchangeRepository
    {
        private readonly string _connectionString;
        public ExchangeRepository(string connectioString)
        {
            _connectionString = connectioString;
        }

        public async Task<ExchangeRate> GetLatest(Currency currency)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<ExchangeRate>(
                                @"select top 1 
                                    Id, Date, Currency, Rate 
                                from dbo.ExchangeRate 
                                where Currency = @Currency 
                                order by Date desc", new { Currency = currency })
                                .ConfigureAwait(false);
            }
        }

        public async Task Create(ExchangeRate task)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                const string query = @"insert into dbo.ExchangeRate (Currency, Rate)
                                       values (@Currency, @Rate)";
                await db.ExecuteAsync(query, task).ConfigureAwait(false);
            }
        }

        public async Task<DateTime> GetLastDateTime()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteScalarAsync<DateTime?>("SELECT TOP 1 Date FROM dbo.ExchangeRate order by Date desc").ConfigureAwait(false) ?? DateTime.MinValue;
            }
        }
    }
}
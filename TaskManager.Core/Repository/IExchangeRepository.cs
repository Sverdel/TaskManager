using System;
using System.Threading.Tasks;
using TaskManager.Core.Model;

namespace TaskManager.Core.Repository
{
    public interface IExchangeRepository
    {
        Task Create(ExchangeRate task);

        Task<ExchangeRate> GetLatest(Currency currency);

        Task<DateTime> GetLastDateTime();
    }
}
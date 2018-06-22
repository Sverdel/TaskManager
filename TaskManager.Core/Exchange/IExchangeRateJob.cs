using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Core.Model;

namespace TaskManager.Core.Exchange
{
    public interface IExchangeRateJob
    {
        Task ExecuteAsync(Action<ExchangeRate> onProcessed, CancellationToken stoppingToken = default);
    }
}
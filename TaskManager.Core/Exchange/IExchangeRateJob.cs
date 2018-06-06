using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.Core.Exchange
{
    public interface IExchangeRateJob
    {
        Task Execute(CancellationToken stoppingToken = default);
    }
}
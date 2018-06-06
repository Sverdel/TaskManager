using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TaskManager.Core.Exchange;

namespace TaskManager.Api.Services
{
    public class ExchangeService : BackgroundService
    {
        private readonly IExchangeRateJob _job;

        public ExchangeService(IExchangeRateJob job)
        {
            _job = job;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => _job.Execute(stoppingToken);
    }
}

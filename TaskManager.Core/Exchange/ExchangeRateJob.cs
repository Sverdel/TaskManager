using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Core.Repository;
using Cronos;

namespace TaskManager.Core.Exchange
{
    public class ExchangeRateJob : IExchangeRateJob
    {
        private readonly IExchangeRepository _repository;
        private readonly IRateGatter _getter;
        private readonly CronExpression _scheduler;

        public ExchangeRateJob(IExchangeRepository repository, IRateGatter getter)
        {
            _repository = repository;
            _getter = getter;
            _scheduler = CronExpression.Parse("0 * * * *");
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastRun = (await _repository.GetLastDateTime().ConfigureAwait(false)).ToUniversalTime();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await _scheduler.WaitNextOccurrenceAsync(lastRun, stoppingToken).ConfigureAwait(false);
                lastRun = DateTime.UtcNow;
                foreach (var rate in await _getter.GetRates().ConfigureAwait(false))
                {
                    await _repository.Create(rate).ConfigureAwait(false);
                }
            }
        }
    }
}

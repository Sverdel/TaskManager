using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Core.Repository;
using Cronos;
using TaskManager.Core.Model;

namespace TaskManager.Core.Exchange
{
    public class ExchangeRateJob : IExchangeRateJob
    {
        private readonly IExchangeRepository _repository;
        private readonly IRateGatter _getter;
        private readonly CronExpression _scheduler;
        private readonly Random rnd = new Random((int)DateTime.Now.Ticks);

        public ExchangeRateJob(IExchangeRepository repository, IRateGatter getter)
        {
            _repository = repository;
            _getter = getter;
            _scheduler = CronExpression.Parse("0 * * * *");
        }

        public async Task ExecuteAsync(Action<ExchangeRate> onProcessed, CancellationToken stoppingToken)
        {
            var lastRun = (await _repository.GetLastDateTime().ConfigureAwait(false)).ToUniversalTime();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await _scheduler.WaitNextOccurrenceAsync(lastRun, stoppingToken).ConfigureAwait(false);
                lastRun = DateTime.UtcNow;

                foreach (var rate in await _getter.GetRates().ConfigureAwait(false))
                {
                    onProcessed(rate);
                    await _repository.Create(rate).ConfigureAwait(false);
                }
            }
        }
    }
}

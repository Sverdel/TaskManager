using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Core.Repository;

namespace TaskManager.Core.Exchange
{
    public class ExchangeRateJob : IExchangeRateJob
    {
        private readonly IExchangeRepository _repository;
        private readonly IRateGatter _getter;

        public ExchangeRateJob(IExchangeRepository repository, IRateGatter getter)
        {
            _repository = repository;
            _getter = getter;
        }

        public async Task Execute(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var date = await _repository.GetLastDateTime().ConfigureAwait(false);
                if (!date.HasValue || date - DateTime.Now > TimeSpan.FromHours(1))
                {
                    foreach (var rate in await _getter.GetRates().ConfigureAwait(false))
                    {
                        await _repository.Create(rate).ConfigureAwait(false);
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}

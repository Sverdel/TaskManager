using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using TaskManager.Api.Hubs;
using TaskManager.Api.Models.Dto;
using TaskManager.Core.Exchange;
using TaskManager.Core.Model;

namespace TaskManager.Api.Services
{
    public class ExchangeService : BackgroundService
    {
        private readonly IExchangeRateJob _job;
        private readonly IHubContext<ExchangeHub> _exchangeHub;
        private readonly Action<ExchangeRate> _onProcessed;

        public ExchangeService(IExchangeRateJob job, IHubContext<ExchangeHub> exchangeHub)
        {
            _job = job;
            _exchangeHub = exchangeHub;
            _onProcessed = async rate => await _exchangeHub.Clients.All.SendAsync("rateChanged", Mapper.Map<ExchangeRateDto>(rate)).ConfigureAwait(false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => _job.ExecuteAsync(_onProcessed, stoppingToken);
    }
}

using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using TaskManager.Core.Exchange;
using TaskManager.Core.Model;
using TaskManager.Core.Repository;

namespace TaskManager.Core
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddHttpClient(RateGatter.ExchangeClientName, c =>
                    {
                        c.BaseAddress = new Uri("http://data.fixer.io/api/latest");
                    })
                    .AddTransientHttpErrorPolicy(p => p.RetryAsync(3))
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddTransient<IRepository<WorkTask, long>>(serv => new TaskRepository(connectionString));
            services.AddTransient<IRepository<State, int>>(serv => new StateRepository(connectionString));
            services.AddTransient<IRepository<Priority, int>>(serv => new PriorityRepository(connectionString));
            services.AddTransient<IExchangeRepository>(serv => new ExchangeRepository(connectionString));
            services.AddTransient<IRateGatter, RateGatter>();
            services.AddTransient<IExchangeRateJob, ExchangeRateJob>();
        }
    }
}

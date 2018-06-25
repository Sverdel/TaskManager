using TaskManager.Api.Models.Dto;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace TaskManager.Api.Hubs
{
    public class ExchangeHub : Hub<IExchangeHub>
    {
        public void rateChanged(ExchangeRateDto rate)
        {
            Clients.AllExcept(new List<string> { Context.ConnectionId }).rateChanged(rate);
        }
    }
}
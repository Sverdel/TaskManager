using TaskManager.Api.Models.Dto;

namespace TaskManager.Api.Hubs
{
    public interface IExchangeHub
    {
        void rateChanged(ExchangeRateDto rate);
    }
}
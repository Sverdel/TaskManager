using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Core.Model;

namespace TaskManager.Core.Exchange
{
    public class RateGatter : IRateGatter
    {
        public const string ExchangeClientName = "exchange";
        private const string _apiKey = "d6d2774b2a7c7db43b6db1bc267cfc73";
        private const string _symbols = "RUB,USD";
        private readonly IHttpClientFactory _clientFactory;

        public RateGatter(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<ExchangeRate>> GetRates()
        {
            using (var client = _clientFactory.CreateClient(ExchangeClientName))
            {
                var response = await client.GetAsync($"?access_key={_apiKey}&format=1&symbols={_symbols}").ConfigureAwait(false);
                var rates = await response.Content.ReadAsAsync<ExchangeRates>().ConfigureAwait(false);
                return Mapper.Map<IEnumerable<ExchangeRate>>(rates);
            }
        }
    }
}

using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManager.ExchangeRate.Model;

namespace TaskManager.ExchangeRate
{
    public class ExchangeRateClient
    {
        private const string _baseUrl = "https://www.cbr-xml-daily.ru/";
        private IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private TimeSpan _expiration = TimeSpan.FromHours(1);
        private readonly List<string> _currencies = new List<string> { "AUD", "AZN", "GBP", "AMD", "BYN", "BGN", "BRL", "HUF", "HKD", "DKK", "USD", "EUR", "INR", "KZT", "CAD", "KGS", "CNY", "MDL", "NOK", "PLN", "RON", "XDR", "SGD", "TJS", "TRY", "TMT", "UZS", "UAH", "CZK", "SEK", "CHF", "ZAR", "KRW", "JPY" };

        public async Task<double> GetRateAsync(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentNullException("");
            }
            if (!_currencies.Contains(currency.ToUpper()))
            {
                throw new ArgumentException("Unsupported currency");
            }

            if (!_cache.TryGetValue(DateTime.Now.Date, out RateSet rate))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    HttpResponseMessage response = await client.GetAsync("daily_json.js").ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException("Rate exchange service is unavailible");
                    }

                    string jsondata = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    rate = JsonConvert.DeserializeObject<RateSet>(jsondata);

                    _cache.Set(DateTime.Now.Date, rate, _expiration);
                }
            }

            return rate.Valute[currency.ToUpper()].Value;
        }
    }
}

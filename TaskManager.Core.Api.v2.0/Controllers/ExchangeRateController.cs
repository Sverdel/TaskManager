using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;

namespace TaskManager.Core.Api.v2._0.Controllers
{
    [Produces("application/json")]
    [Route("api/rate")]
    public class ExchangeRateController : Controller
    {
        private const string _baseUrl = "https://www.cbr-xml-daily.ru/";
        private readonly IMemoryCache _cache;
        private TimeSpan _expiration = TimeSpan.FromHours(1);
        private readonly List<string> _currencies = new List<string> { "AUD", "AZN", "GBP", "AMD", "BYN", "BGN", "BRL", "HUF", "HKD", "DKK", "USD", "EUR", "INR", "KZT", "CAD", "KGS", "CNY", "MDL", "NOK", "PLN", "RON", "XDR", "SGD", "TJS", "TRY", "TMT", "UZS", "UAH", "CZK", "SEK", "CHF", "ZAR", "KRW", "JPY" };

        public ExchangeRateController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet("{currency}")]
        public async Task<IActionResult> GetUsdRate([FromRoute]string currency)
        {
            if (!_currencies.Contains(currency.ToUpper()))
            {
                return BadRequest("Unsupported currency");
            }

            if (!_cache.TryGetValue(DateTime.Now.Date, out ExchangeRate rate))
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_baseUrl);
                        HttpResponseMessage response = await client.GetAsync("daily_json.js").ConfigureAwait(false);
                        if (!response.IsSuccessStatusCode)
                        {
                            return BadRequest("Rate exchange service is unavailible");
                        }

                        string jsondata = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        rate = JsonConvert.DeserializeObject<ExchangeRate>(jsondata);

                        _cache.Set(DateTime.Now.Date, rate, _expiration);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return Ok(rate.Valute[currency.ToUpper()].Value);
        }
    }
}
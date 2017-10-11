using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaskManager.Core.Api.Models.DataModel;
using TaskManager.ExchangeRate;

namespace TaskManager.Core.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/rate")]
    public class ExchangeRateController : Controller
    {
        private readonly Lazy<ExchangeRateClient> _rater;

        public ExchangeRateController()
        {
            _rater = new Lazy<ExchangeRateClient>(new ExchangeRateClient());
        }

        [HttpGet("{currency}")]
        public async Task<IActionResult> GetRate([FromRoute]string currency)
        {
            try
            {
                return Ok(await _rater.Value.GetRateAsync(currency).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
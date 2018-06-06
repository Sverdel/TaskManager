using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models.Dto;
using TaskManager.Core.Model;
using TaskManager.Core.Repository;

namespace TaskManager.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/rate")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRepository _repository;

        public ExchangeRateController(IExchangeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{currency}")]
        public async Task<ActionResult<ExchangeRateDto>> Get(string currency)
        {
            if (!Enum.TryParse<Currency>(currency, out var currencyEnum))
            {
                return BadRequest("Incompatible currency");
            }

            return Mapper.Map<ExchangeRateDto>(await _repository.GetLatest(currencyEnum));
        }
    }
}
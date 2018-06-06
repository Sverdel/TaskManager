using System;
using System.Collections.Generic;
using AutoMapper;
using TaskManager.Core.Model;

namespace TaskManager.Core
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ExchangeRates, IEnumerable<ExchangeRate>>().ConvertUsing<ExchangeRateConverter>();
        }
    }

    public class ExchangeRateConverter : ITypeConverter<ExchangeRates, IEnumerable<ExchangeRate>>
    {
        public IEnumerable<ExchangeRate> Convert(ExchangeRates source, IEnumerable<ExchangeRate> destination, ResolutionContext context)
        {
            if (!source.Success)
                yield break;

            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
            {
                yield return new ExchangeRate
                {
                    Date = DateTime.Now,
                    Currency = currency,
                    Rate = source.Rates.GetRate(currency)
                };
            }
        }
    }
}

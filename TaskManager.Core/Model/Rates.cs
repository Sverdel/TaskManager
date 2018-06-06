using System;

namespace TaskManager.Core.Model
{
    public class Rates
    {
        public decimal USD { get; set; }
        public decimal RUB { get; set; }

        public decimal GetRate(Currency currency)
        {
            switch (currency)
            {
                case Currency.USD:
                    return RUB / USD;
                case Currency.EUR:
                    return RUB;
                default:
                    throw new ArgumentException("Incompatible currency");
            }
        }
    }

}

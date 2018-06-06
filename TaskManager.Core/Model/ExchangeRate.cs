using System;

namespace TaskManager.Core.Model
{
    public class ExchangeRate
    {
        public int? Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Rate { get; set; }

        public Currency Currency { get; set; }
    }
}

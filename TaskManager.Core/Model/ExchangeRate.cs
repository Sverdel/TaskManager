using System;
using System.ComponentModel.DataAnnotations.Schema;

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

using System;
using System.Collections.Generic;

namespace TaskManager.ExchangeRate.Model
{
    public class RateSet
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, Rate> Valute { get; set; }
    }
}

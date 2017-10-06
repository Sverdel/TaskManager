using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TaskManager.Core.Api.Models.DataModel
{
    //public class ExchangeRate
    //{
    //    [JsonProperty("base")]
    //    public string Base { get; set; }

    //    [JsonProperty("date")]
    //    public DateTime Date { get; set; }

    //    [JsonProperty("rates")]
    //    public Dictionary<string, float> Rates { get; set; }
    //}


    public class ExchangeRate
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, Rate> Valute { get; set; }
    }

    public class Rate
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public float Previous { get; set; }
    }


}

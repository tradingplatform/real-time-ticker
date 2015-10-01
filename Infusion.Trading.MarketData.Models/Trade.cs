using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infusion.Trading.MarketData.Models
{
    public class Trade
    {
        public string TradeId { get; set; }
        public DateTime TradeTime { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public decimal FillPrice { get; set; }
        public int FillQuantity { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Side Side { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

    }
}
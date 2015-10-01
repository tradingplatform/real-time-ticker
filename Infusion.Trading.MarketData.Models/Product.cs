using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infusion.Trading.MarketData.Models
{
    public class Product
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType ProductType { get; set; }
        public string ProducctName { get; set; }
        public int LotSize { get; set; }
        public int TickSize { get; set; }
        public string Series { get; set; }
        public decimal StrikePrice { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public OptionType? OptionType { get; set; }
    }
}
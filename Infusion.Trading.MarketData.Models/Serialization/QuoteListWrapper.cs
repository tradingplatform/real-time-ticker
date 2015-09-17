using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infusion.Trading.MarketData.Models.Serialization
{
    public class QuoteListWrapper
    {
        [JsonConverter(typeof(SingleOrArrayConverter<Quote>))]
        public IList<Quote> quote { get; set; }
    }
}
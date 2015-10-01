using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infusion.Trading.MarketData.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public Client Client { get; set; }
        public Trader Trader { get; set; }
        public string ExchangeOrderId { get; set; }
        public DateTime ExchangeCreateTime { get; set; }
        public DateTime ExchangeLastChangeTime { get; set; }
        public DateTime AsOf { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Side Side { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal StopPrice { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderDurationType OrderDurationType { get; set; }

        public IList<OrderFill> OrderFills { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProductType SecurityType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType TransactionType { get; set; }
    }
}

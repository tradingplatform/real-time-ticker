using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public OrderType OrderType { get; set; }
        public Side Side { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal StopPrice { get; set; }

        public OrderStatus Status { get; set; }

        public OrderDurationType OrderDurationType { get; set; }

        public IList<OrderFill> OrderFills { get; set; }
        public ProductType SecurityType { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}

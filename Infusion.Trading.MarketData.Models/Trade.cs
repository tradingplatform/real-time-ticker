using System;

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
        public Side Side { get; set; }
        public OrderType OrderType { get; set; }

    }
}
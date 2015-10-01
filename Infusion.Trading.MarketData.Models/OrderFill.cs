using System;

namespace Infusion.Trading.MarketData.Models
{
    public class OrderFill
    {
        public string OrderFillId { get; set; }
        public DateTime AsOf { get; set; }

        public string BuyOrderId { get; set; }
        public string SellOrderId { get; set; }
        public DateTime FillDate { get; set; }
        public int FillQuantity { get; set; }
    }
}
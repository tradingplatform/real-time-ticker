namespace Infusion.Trading.MarketData.Models
{
    public class Position
    {
        public Product Product { get; set; }
        public Side Side { get; set; }
        public int Quantity { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
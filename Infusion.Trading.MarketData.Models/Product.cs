namespace Infusion.Trading.MarketData.Models
{
    public class Product
    {
        public ProductType ProductType { get; set; }
        public string ProducctName { get; set; }
        public int LotSize { get; set; }
        public int TickSize { get; set; }
        public string Series { get; set; }
        public decimal StrikePrice { get; set; }
        public OptionType? OptionType { get; set; }
    }
}
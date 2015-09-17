using System;

namespace Infusion.Trading.MarketData.Models
{
    public class Quote
    {
        public string SecurityId { get; set; }
        public string Exchange { get; set; }
        public decimal DayOpen { get;  set; }
        public decimal DayLow { get; set; }
        public decimal DayHigh { get; set; }
        public decimal LastChange { get; set; }
        public decimal Change => Price - DayOpen;
        public double PercentChange => (double)Math.Round(Change / Price, 4);
        public decimal Price { get; set; }
        public DateTime AsOf { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Infusion.Trading.MarketData.Models;

namespace Infusion.Trading.MarketData.CoreServices.Contracts
{
    public interface IZmqService
    {
        event Action<IList<Quote>> HandleTickerInfoPublish;
        void SubscribeTicker(string ticker);
        void Start();
    }
}
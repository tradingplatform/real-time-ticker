using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using TestStack.Dossier;
using TestStack.Dossier.Lists;

namespace Infusion.Trading.MarketData.Models.Tests
{
    public class ClientBuilder : TestDataBuilder<Client, ClientBuilder> { }

    public class OrderBuilder : TestDataBuilder<Order, OrderBuilder>{ }

    public class TradeBuilder : TestDataBuilder<Trade, TradeBuilder> { }

    public class TraderBuilder : TestDataBuilder<Trader, TraderBuilder> { }

    public class OrderFillsBuilder : TestDataBuilder<OrderFill, OrderFillsBuilder> { }

    public class ProductBuilder : TestDataBuilder<Product, ProductBuilder> { }

    [TestFixture]
    public class DomainModelTests
    {
        [Test]
        public void ShouldGenerateOrderJson()
        {
            var order = new OrderBuilder()
                .Set(x=>x.Client, new ClientBuilder().Build())
                .Set(x => x.Trader, new TraderBuilder().Build())
                .Build();
            var payload = JsonConvert.SerializeObject(order, Formatting.Indented);
            var dir = TestContext.CurrentContext.WorkDirectory;
            var saveFile = Path.Combine(dir, "order.json");
            //File.WriteAllText(saveFile, payload);
            var copyDir = Path.Combine(dir, "..\\..\\Artifacts\\order.json");
            File.WriteAllText(copyDir, payload);
        }

        [Test]
        public void ShouldGenerateTradeJson()
        {
            var trade = new TradeBuilder()
                .Set(x=>x.Order, new OrderBuilder().Build)
                .Set(x=>x.Product, new ProductBuilder().Build)
                .Build();
            var payload = JsonConvert.SerializeObject(trade, Formatting.Indented);
            var dir = TestContext.CurrentContext.WorkDirectory;
            var saveFile = Path.Combine(dir, "trade.json");
            //File.WriteAllText(saveFile, payload);
            var copyDir = Path.Combine(dir, "..\\..\\Artifacts\\trade.json");
            File.WriteAllText(copyDir, payload);
        }
    }
}

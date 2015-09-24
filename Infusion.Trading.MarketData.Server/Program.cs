using Microsoft.Practices.Unity;
using Infusion.Trading.MarketData.CoreServices.Services;
using Infusion.Trading.MarketData.CoreServices.Unity;

namespace Infusion.Trading.MarketData.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            InfusionBootstrapper.Instance.Container.Resolve<MarketDataService>().Start();
            for (;;) { }
        }
    }
}

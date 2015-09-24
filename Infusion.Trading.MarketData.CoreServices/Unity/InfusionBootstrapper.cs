using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices.Contracts;
using Infusion.Trading.MarketData.CoreServices.Services;
using Microsoft.Practices.Unity;

namespace Infusion.Trading.MarketData.CoreServices.Unity
{
    public class InfusionBootstrapper
    {
        private readonly string pluginsDirectory = Path.Combine(Environment.CurrentDirectory, ".");
        private readonly Lazy<List<Assembly>> assemblies;

        public static readonly InfusionBootstrapper Instance = new InfusionBootstrapper();

        private readonly IUnityContainer unityContainer;

        // initialize the registrations
        public InfusionBootstrapper()
        {
            assemblies = new Lazy<List<Assembly>>(() =>
                Directory.EnumerateFiles(pluginsDirectory, "Infusion*.dll")
                    .Select(Assembly.LoadFrom)
                    .ToList());

            unityContainer = new UnityContainer();
            Register();
        }

        public IUnityContainer Container => unityContainer;

        private void Register()
        {
            if (!unityContainer.IsRegistered<IQuoteService>())
            {
                unityContainer.RegisterType<IQuoteService, YahooFinancialDataService>(new ContainerControlledLifetimeManager());
            }

            if (!unityContainer.IsRegistered<IZmqService>())
            {
                unityContainer.RegisterType<IZmqService, ZmqService>(new ContainerControlledLifetimeManager());
            }
        }
    }
}

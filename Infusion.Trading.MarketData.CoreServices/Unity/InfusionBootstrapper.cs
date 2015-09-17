using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infusion.Trading.MarketData.CoreServices.Unity
{
    public class InfusionBootstrapper
    {
        private static readonly string pluginsDirectory = Path.Combine(Environment.CurrentDirectory, ".");
        private static readonly Lazy<List<Assembly>> assemblies = new Lazy<List<Assembly>>(() =>
            Directory.EnumerateFiles(pluginsDirectory, "Infusion*.dll")
                     .Select(Assembly.LoadFrom)
                     .ToList());

        public static readonly InfusionBootstrapper Instance = new InfusionBootstrapper();

        // initialize the registrations
        static InfusionBootstrapper()
        {

        }
    }
}

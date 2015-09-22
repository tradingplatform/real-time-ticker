using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.Client;

namespace MarketDataTestBench
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || !args.Any())
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Usage: MarketDataTestBench [snapshot] symbol symbol ...");
                return;
            }

            using (var client = new ZmqClient())
            {
                if (args[0] == "snapshot")
                {
                    var response = client.GetQuoteInfoRaw(args.Skip(1).ToArray());
                    Console.WriteLine(response);
                    Console.WriteLine();
                    return;
                }

                client.StartConsole(args);
            }
        }
    }
}

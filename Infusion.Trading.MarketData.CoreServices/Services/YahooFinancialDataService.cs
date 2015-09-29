using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices.Contracts;
using Infusion.Trading.MarketData.Models;
using Infusion.Trading.MarketData.Models.Serialization;
using Newtonsoft.Json;

namespace Infusion.Trading.MarketData.CoreServices.Services
{
    public class YahooFinancialDataService : IQuoteService
    {
        private readonly Uri baseUrl = new Uri("http://query.yahooapis.com/");

        public IList<Quote> GetQuotes(params string[] securityIds)
        {
            return GetQuotesInternal(securityIds).Result;
        }

        private async Task<IList<Quote>> GetQuotesInternal(params string[] securityIds)
        {
            // http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22MSFT%22%2C%22GOOG%22%2C%22AAPL%22)&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&format=json
            if (securityIds == null || !securityIds.Any()) return new List<Quote>();

            var result = new List<Quote>();

            var securityIdsAsString = string.Join("%2C", securityIds.Select(s=> $"\"{s}\""));
            var selectSql = $"select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({securityIdsAsString})";
            var queryUri = $"v1/public/yql?q={selectSql}&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&format=json";

            using (var client = new HttpClient { BaseAddress = baseUrl })
            {
                var response = await client.GetStringAsync(queryUri).ConfigureAwait(false);
                var deserializedResponse = JsonConvert.DeserializeObject<QuoteResponse>(response);
                
                var asOf = DateTime.Now;
                result.AddRange(
                    deserializedResponse.query.results.quote.Select(q =>
                    {
                        q.AsOf = asOf;
                        q.LastTradeDateAsString = q.LastTradeDate?.ToString("MM/dd/yyyy") + " " + q.LastTradeTime;
                        return q;
                    }));
            }

            return result;
        }
    }
}

using MicroserviceSample.Currencies.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroserviceSample.CurrencyAPIs.Services
{
    //https://openexchangerates.org/api/currencies.json
    public interface ICurrencyService
    {
        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CurrencyNamesView>> GetAll();
    }

    public class CurrencyService : ICurrencyService
    {
        public HttpClient client;

        public CurrencyService(HttpClient httpClient)
        {
            client = httpClient;
        }

        public async Task<IEnumerable<CurrencyNamesView>> GetAll()
        {
            List<CurrencyNamesView> currencyList = new List<CurrencyNamesView>();
            try
            {
                var response = await client.GetStringAsync("https://openexchangerates.org/api/currencies.json");
                var currencies = JsonConvert.DeserializeObject<dynamic>(response);
                string currencyStr = currencies != null ? JsonConvert.SerializeObject(currencies) : "";
                currencyStr = currencyStr.Replace("\\", "").Replace("\"", "");
                currencyStr = currencyStr.Replace("{", "");
                currencyStr = currencyStr.Replace("}", "");
                currencyList = this.GetCurrencyNames(currencyStr);
                string currencyJsonStr = JsonConvert.SerializeObject(currencyStr);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return await Task<CurrencyNamesView>.Run(() => currencyList).ConfigureAwait(false);
        }

        private List<CurrencyNamesView> GetCurrencyNames(string ratesStr)
        {
            List<CurrencyNamesView> namesList = new List<CurrencyNamesView>();
            if (ratesStr.Length > 0)
            {
                string[] rateRows = ratesStr.Split(",");
                for (int row = 0; row < rateRows.Length; row++)
                {
                    string[] currencyRate = rateRows[row].Split(":");
                    if (currencyRate.Length > 1)
                    {
                        string code = currencyRate[0];
                        string currency = currencyRate[1];

                        namesList.Add(new CurrencyNamesView()
                        {
                            Currency = currency,
                            Code = code
                        });
                    }
                }
            }
            return namesList;
        }
    }
}

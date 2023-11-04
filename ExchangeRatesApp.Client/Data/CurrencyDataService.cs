using ExchangeRatesApp.Models;
using Serilog;
using System.Net.Http.Json;

namespace ExchangeRatesApp.Client.Data
{
    public class CurrencyDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables()
        {
            //var httpClient = _httpClientFactory.CreateClient();
            //httpClient.BaseAddress = new Uri("https://api.nbp.pl/");

            var tables = new List<string> { "a", "b", "c" };
            var allCurrencies = new List<CurrencyRates>();

            foreach (var table in tables)
            {
                var currencyRates = await GetAllCurrencies(table);
                allCurrencies.AddRange(currencyRates);
            }

            return allCurrencies;
        }

        public async Task<List<CurrencyRates>> GetAllCurrencies(string table)
        {
            var httpClient = _httpClientFactory.CreateClient("NBPClient");
            //httpClient.BaseAddress = new Uri("https://api.nbp.pl/");

            var response = await httpClient.GetAsync($"api/exchangerates/tables/{table}/");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CurrencyRates>>();
            }
            else
            {
                throw new Exception("Błąd podczas pobierania danych z API.");
            }
        }
    }
}

using ExchangeRatesApp.Models;
using ExchangeRatesApp.Models.RatesChooseDate;
using Newtonsoft.Json;
using Serilog;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ExchangeRatesApp.Client.Data
{
    public class CurrencyDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Rate>> GetAllRatesFromAllTables()
        {
            var tables = new List<string> { "a", "b" };
            var allRates = new List<Rate>();

            foreach (var table in tables)
            {
                var currencyRates = await GetAllCurrencies(table);

                // Wydobywanie Rates z każdego CurrencyRates i dodawanie ich do allRates
                foreach (var currencyRate in currencyRates)
                {
                    allRates.AddRange(currencyRate.Rates);
                }
            }

            //var plnRate = new Rate
            //{
            //    Currency = "Polski Złoty",
            //    Code = "PLN",
            //    Mid = 1.0
            //};

            //// Dodawanie Rates dla PLN
            //allRates.Add(plnRate);

            return allRates;
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
                HandleApiError();
                return new List<CurrencyRates>();
            }
        }

        public async Task<ExchangeRatesSeries> GetLastXCurrencies(string code, int topCount)
        {
            var tables = new[] { "A", "B" };

            foreach (var table in tables)
            {
                var httpClient = _httpClientFactory.CreateClient("NBPClient");
                var exchangeRate = await httpClient.GetFromJsonAsync<ExchangeRatesSeries>($"api/exchangerates/rates/{table}/{code}/last/{topCount}/");

                if (exchangeRate?.Code == code)
                {
                    exchangeRate.Table = table;
                    return exchangeRate;
                }
            }

            return null;
        }

        private void HandleApiError()
        {
            throw new Exception("Błąd podczas pobierania danych z API.");
        }
    }
}

using ExchangeRatesApp.Client.Helpers;
using ExchangeRatesApp.Models;
using Serilog;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ExchangeRatesApp.Client.Data
{
    public class CurrencyRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<CurrencyRates>> GetAllCurrencies(string table)
        {
            using (var httpClient = _httpClientFactory.CreateClient("NBPClient"))
            {
                httpClient.BaseAddress = new Uri("https://api.nbp.pl/");

                try
                {
                    var response = await httpClient.GetAsync($"api/exchangerates/tables/{table}/");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadFromJsonAsync<List<CurrencyRates>>();
                        return content ?? new List<CurrencyRates>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ExceptionHandler.HandleBadRequest(ex.Message);
                }
            }

            return new List<CurrencyRates>();
        }


        public async Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables()
        {
            var tables = new List<string> { "a", "b" };
            var allCurrencies = new List<CurrencyRates>();

            foreach (var table in tables)
            {
                var currencyRates = await GetAllCurrencies(table);

                if (table == "a")
                {
                    currencyRates.Insert(0, new CurrencyRates
                    {
                        Table = table,
                        Rates = new List<Rate>
                        {
                            new Rate
                            {
                                Currency = "polski złoty",
                                Code = "PLN",
                                Mid = 1.0m
                            }
                        }
                    });
                }

                allCurrencies.AddRange(currencyRates);
            }

            return allCurrencies;
        }


        public async Task<CurrencyRates?> GetExchangeRatesOnDate(string code, DateTime date)
        {
            using (var httpClient = _httpClientFactory.CreateClient("NBPClient"))
            {
                httpClient.BaseAddress = new Uri("https://api.nbp.pl/");
                var table = await GetTable(code, httpClient);

                if (table == null)
                {
                    ExceptionHandler.HandleEmptyTable();
                }

                var apiUrl = $"api/exchangerates/rates/{table}/{code}/{date:yyyy-MM-dd}";
                var result = await httpClient.GetFromJsonAsync<CurrencyRates>(apiUrl);

                if (result == null)
                {
                    ExceptionHandler.HandleNotFound();
                }

                return result;
            }
        }

        public async Task<CurrencyRates?> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate)
        {
            using (var httpClient = _httpClientFactory.CreateClient("NBPClient"))
            {
                httpClient.BaseAddress = new Uri("https://api.nbp.pl/");
                var table = await GetTable(code, httpClient);

                if (table == null)
                {
                    ExceptionHandler.HandleEmptyTable();
                }

                var apiUrl = $"api/exchangerates/rates/{table}/{code}/{startDate.ToString("yyyy-MM-dd")}/{endDate.ToString("yyyy-MM-dd")}";
                var response = await httpClient.GetFromJsonAsync<CurrencyRates>(apiUrl);

                return response;
            }
        }

        public async Task<string?> GetTable(string code, HttpClient httpClient)
        {
            var table = await TryGetRatesFromTable(code, httpClient, "A") ?? await TryGetRatesFromTable(code, httpClient, "B");
            return table;
        }

        public async Task<string?> TryGetRatesFromTable(string code, HttpClient httpClient, string table)
        {
            using (var client = _httpClientFactory.CreateClient("NBPClient"))
            {
                client.BaseAddress = new Uri("https://api.nbp.pl/");
                var apiUrl = $"api/exchangerates/rates/{table}/{code}";
                try
                {
                    var response = await client.GetFromJsonAsync<CurrencyRates>(apiUrl);
                    if (response != null)
                    {
                        return table;
                    }
                }
                catch (Exception)
                {
                }
            }

            return null;
        }
    }
}

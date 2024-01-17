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
            var httpClient = _httpClientFactory.CreateClient("NBPClient");
            var exchangeRatesA = await TryGetRatesFromTable(httpClient, "A", code, topCount);

            if (exchangeRatesA == null)
            {
                return await TryGetRatesFromTable(httpClient, "B", code, topCount);
            }

            return exchangeRatesA;
        }

        private async Task<ExchangeRatesSeries> TryGetRatesFromTable(HttpClient httpClient, string table, string code, int topCount)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<ExchangeRatesSeries>($"https://api.nbp.pl/api/exchangerates/rates/{table}/{code}/last/{topCount}/");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<ExchangeRatesSeries> GetExchangeRatesOnDate(string code, DateTime date)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var table = await TryGetRatesFromTable(code, httpClient, "A");
            if (table == null)
            {
                table = await TryGetRatesFromTable(code, httpClient, "B");
            }

            if (table == null)
            {
                return null;
            }

            var apiUrl = $"https://api.nbp.pl/api/exchangerates/rates/{table}/{code}/{date.ToString("yyyy-MM-dd")}";
            var response = await httpClient.GetFromJsonAsync<ExchangeRatesSeries>(apiUrl);

            return response;
        }

        public async Task<ExchangeRatesSeries> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var table = await TryGetRatesFromTable(code, httpClient, "A");
            if (table == null)
            {
                table = await TryGetRatesFromTable(code, httpClient, "B");
            }

            if (table == null)
            {
                return null;
            }

            var apiUrl = $"https://api.nbp.pl/api/exchangerates/rates/{table}/{code}/{startDate.ToString("yyyy-MM-dd")}/{endDate.ToString("yyyy-MM-dd")}";
            var response = await httpClient.GetFromJsonAsync<ExchangeRatesSeries>(apiUrl);

            return response;
        }

        private async Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table)
        {
            var apiUrl = $"https://api.nbp.pl/api/exchangerates/rates/{table}/{code}";
            try
            {
                var response = await httpClient.GetFromJsonAsync<ExchangeRatesSeries>(apiUrl);
                if (response != null)
                {
                    return table;
                }
            }
            catch (HttpRequestException)
            {
                // Ignoruj błędy związane z brakiem danych w danej tabeli
            }

            return null;
        }

        private void HandleApiError()
        {
            throw new Exception("Błąd podczas pobierania danych z API.");
        }
    }
}

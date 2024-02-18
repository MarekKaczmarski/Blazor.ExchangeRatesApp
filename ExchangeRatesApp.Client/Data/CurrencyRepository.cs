using ExchangeRatesApp.Client.Helpers;
using ExchangeRatesApp.Models;
using Serilog;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ExchangeRatesApp.Client.Data
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string BaseApiUrl = "https://api.nbp.pl/";
        private const string RatesApiPath = "api/exchangerates/rates/";
        private const string TablesApiPath = "api/exchangerates/tables/";

        public CurrencyRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient ConfigureHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient("NBPClient");
            httpClient.BaseAddress = new Uri(BaseApiUrl);
            return httpClient;
        }

        public async Task<List<CurrencyRates>> GetAllCurrencies(string table)
        {
            try
            {
                using var httpClient = ConfigureHttpClient();
                var response = await httpClient.GetAsync($"{TablesApiPath}{table}/");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<CurrencyRates>>() ?? new List<CurrencyRates>();
                }
            }
            catch (HttpRequestException ex)
            {
                ExceptionHandler.HandleBadRequest(ex.Message);
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
                allCurrencies.AddRange(currencyRates);
            }

            return allCurrencies;
        }

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromAllTablesWithPLN()
        {
            var tables = new List<string> { "a", "b" };
            var allCurrencies = new List<CurrencyRates>();

            foreach (var table in tables)
            {
                var currencyRates = await GetAllCurrencies(table);

                if (table == "a")
                {
                    currencyRates.Insert(0, CreatePLNCurrencyRates(table));
                }

                allCurrencies.AddRange(currencyRates);
            }

            return allCurrencies;
        }

        private CurrencyRates CreatePLNCurrencyRates(string table)
        {
            return new CurrencyRates
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
            };
        }

        public async Task<CurrencyRates?> GetExchangeRatesOnDate(string code, DateTime date)
        {
            using var httpClient = ConfigureHttpClient();
            var table = await GetTable(code, httpClient);

            if (table == null)
            {
                ExceptionHandler.HandleEmptyTable();
            }

            var apiUrl = $"{RatesApiPath}{table}/{code}/{date:yyyy-MM-dd}";
            var result = await httpClient.GetFromJsonAsync<CurrencyRates>(apiUrl);

            if (result == null)
            {
                ExceptionHandler.HandleNotFound();
            }

            return result;
        }

        public async Task<CurrencyRates?> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate)
        {
            using var httpClient = ConfigureHttpClient();
            var table = await GetTable(code, httpClient);

            if (table == null)
            {
                ExceptionHandler.HandleEmptyTable();
            }

            var apiUrl = $"{RatesApiPath}{table}/{code}/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}";
            var response = await httpClient.GetFromJsonAsync<CurrencyRates>(apiUrl);

            return response;
        }

        public async Task<string?> GetTable(string code, HttpClient httpClient)
        {
            var table = await TryGetRatesFromTable(code, httpClient, "A") ?? await TryGetRatesFromTable(code, httpClient, "B");
            return table;
        }

        public async Task<string?> TryGetRatesFromTable(string code, HttpClient httpClient, string table)
        {
            var apiUrl = $"{RatesApiPath}{table}/{code}";

            try
            {
                var response = await httpClient.GetFromJsonAsync<CurrencyRates>(apiUrl);

                if (response != null)
                {
                    return table;
                }
            }
            catch (Exception)
            {
                
            }

            return null;
        }
    }
}
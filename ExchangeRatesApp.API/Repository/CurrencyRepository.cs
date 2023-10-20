using ExchangeRatesApp.API.Entities;
using System.Text.Json;

namespace ExchangeRatesApp.API.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly HttpClient _httpClient;

        public CurrencyRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.nbp.pl/");
        }

        public async Task<List<CurrencyRates>> GetAllCurrencies(string table)
        {
            var response = await _httpClient.GetAsync($"api/exchangerates/tables/{table}/");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<CurrencyRates>>();
                return data;
            }
            else
            {
                throw new Exception("Błąd podczas pobierania danych z API.");
            }
        }

        public async Task<Rate?> GetCurrencyByCode(string code)
        {
            var tables = new List<string> { "a", "b", "c" }; // Add all the table letters you want to query

            foreach (var table in tables)
            {
                var currencyRates = await GetAllCurrencies(table);

                var specificCurrency = currencyRates
                    .SelectMany(cr => cr.Rates)
                    .FirstOrDefault(rate => rate.Code == code);

                if (specificCurrency != null)
                {
                    return specificCurrency;
                }
            }

            return null; // Return null if the currency is not found in any table
        }

        public async Task<Rate?> GetCurrencyByCode(string table, string code)
        {
            var currencyRates = await GetAllCurrencies(table);

            var specificCurrency = currencyRates
                .SelectMany(cr => cr.Rates)
                .FirstOrDefault(rate => rate.Code == code);

            return specificCurrency;
        }

        //public async Task<List<CurrencyRates>> GetAllCurrencies()
        //{
        //    var response = await _httpClient.GetAsync("api/exchangerates/tables/a/");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = await response.Content.ReadAsStringAsync();

        //        var options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true,
        //        };

        //        List<CurrencyRates> currencyRates = JsonSerializer.Deserialize<List<CurrencyRates>>(json, options);

        //        return currencyRates;
        //    }
        //    else
        //    {
        //        throw new Exception("Błąd podczas pobierania danych z API.");
        //    }
        //}
    }
}

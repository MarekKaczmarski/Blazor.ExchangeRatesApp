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

        public async Task<List<CurrencyRates>> GetAllCurrencies()
        {
            var response = await _httpClient.GetAsync("api/exchangerates/tables/a/");
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
            var currencyRates = await GetAllCurrencies();

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

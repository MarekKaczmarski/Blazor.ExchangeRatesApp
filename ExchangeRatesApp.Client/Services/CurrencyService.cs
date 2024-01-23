using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Models;
using static MudBlazor.CategoryTypes;
using System.Net.Http;

namespace ExchangeRatesApp.Client.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly CurrencyDataService _currencyDataService;

        public CurrencyService(CurrencyDataService currencyDataService)
        {
            _currencyDataService = currencyDataService;
        }

        public async Task<List<CurrencyRates>> GetAllCurrencies(string table)
        {
            return await _currencyDataService.GetAllCurrencies(table);
        }

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables()
        {
            return await _currencyDataService.GetAllCurrenciesFromAllTables();
        }

        public async Task<CurrencyRates> GetExchangeRatesOnDate(string code, DateTime date)
        {
            return await _currencyDataService.GetExchangeRatesOnDate(code, date);
        }

        public async Task<CurrencyRates> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate)
        {
            return await _currencyDataService.GetExchangeRatesInRange(code, startDate, endDate);
        }

        public async Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table)
        {
            return await _currencyDataService.TryGetRatesFromTable(code, httpClient, table);
        }

        public void HandleApiError()
        {
            _currencyDataService.HandleApiError();
        }
    }
}


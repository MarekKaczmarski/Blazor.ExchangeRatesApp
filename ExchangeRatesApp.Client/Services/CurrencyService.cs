using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Models;
using ExchangeRatesApp.Models.RatesChooseDate;
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

        public async Task<List<Rate>> GetAllRatesFromAllTables()
        {
            return await _currencyDataService.GetAllRatesFromAllTables();
        }

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables()
        {
            return await _currencyDataService.GetAllCurrenciesFromAllTables();
        }

        public async Task<List<CurrencyRates>> GetAllCurrencies(string table)
        {
            return await _currencyDataService.GetAllCurrencies(table);
        }

        public async Task<CurrencyRates> GetLastXCurrencies(string code, int topCount)
        {
            return await _currencyDataService.GetLastXCurrencies(code, topCount);
        }

        public async Task<CurrencyRates> TryGetRatesFromTable(HttpClient httpClient, string table, string code, int topCount)
        {
            return await _currencyDataService.TryGetRatesFromTable(httpClient, table, code, topCount);
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

        //

        public async Task<Rate?> GetCurrencyByCode(string code)
        {
            var allCurrencies = await _currencyDataService.GetAllCurrenciesFromAllTables();
            return FindCurrencyByCode(allCurrencies, code);
        }

        public async Task<Rate?> GetCurrencyByCode(string table, string code)
        {
            var currencyRates = await _currencyDataService.GetAllCurrencies(table);
            return FindCurrencyByCode(currencyRates, code);
        }

        public Rate? FindCurrencyByCode(IEnumerable<CurrencyRates> currencies, string code)
        {
            return currencies
                .SelectMany(cr => cr.Rates)
                .FirstOrDefault(rate => rate.Code == code);
        }
        public void HandleApiError()
        {
            _currencyDataService.HandleApiError();
        }
    }
}


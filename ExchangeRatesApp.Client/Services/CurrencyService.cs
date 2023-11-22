using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Models;
using System.Net.Http.Json;

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

        private Rate? FindCurrencyByCode(IEnumerable<CurrencyRates> currencies, string code)
        {
            return currencies
                .SelectMany(cr => cr.Rates)
                .FirstOrDefault(rate => rate.Code == code);
        }

        //public async Task<Rate?> GetCurrencyByCode(string table, string code)
        //{
        //    var currencyRates = await GetAllCurrencies(table);

        //    var specificCurrency = currencyRates
        //        .SelectMany(cr => cr.Rates)
        //        .FirstOrDefault(rate => rate.Code == code);

        //    return specificCurrency;
        //}
    }
}


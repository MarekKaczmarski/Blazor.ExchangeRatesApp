using CurrencyApplication.Client.Data;
using CurrencyApplication.Models;
using static MudBlazor.CategoryTypes;
using System.Net.Http;

namespace CurrencyApplication.Client.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyRates>> GetAllCurrencies(string table)
        {
            return await _currencyRepository.GetAllCurrencies(table);
        }

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromTables()
        {
            return await _currencyRepository.GetAllCurrenciesFromTables();
        }

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromTablesWithPLN()
        {
            return await _currencyRepository.GetAllCurrenciesFromTablesWithPLN();
        }

        public async Task<CurrencyRates> GetCurrencyRatesOnDate(string code, DateTime date)
        {
            return await _currencyRepository.GetCurrencyRatesOnDate(code, date);
        }

        public async Task<CurrencyRates> GetCurrencyRatesInRange(string code, DateTime startDate, DateTime endDate)
        {
            return await _currencyRepository.GetCurrencyRatesInRange(code, startDate, endDate);
        }

        public async Task<string?> GetTable(string code, HttpClient httpClient)
        {
            return await _currencyRepository.GetTable(code, httpClient);
        }

        public async Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table)
        {
            return await _currencyRepository.TryGetRatesFromTable(code, httpClient, table);
        }
    }
}


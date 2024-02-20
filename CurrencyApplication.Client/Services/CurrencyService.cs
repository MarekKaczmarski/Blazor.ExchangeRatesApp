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

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables()
        {
            return await _currencyRepository.GetAllCurrenciesFromAllTables();
        }

        public async Task<List<CurrencyRates>> GetAllCurrenciesFromAllTablesWithPLN()
        {
            return await _currencyRepository.GetAllCurrenciesFromAllTablesWithPLN();
        }

        public async Task<CurrencyRates> GetExchangeRatesOnDate(string code, DateTime date)
        {
            return await _currencyRepository.GetExchangeRatesOnDate(code, date);
        }

        public async Task<CurrencyRates> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate)
        {
            return await _currencyRepository.GetExchangeRatesInRange(code, startDate, endDate);
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


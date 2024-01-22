using ExchangeRatesApp.Models;
using ExchangeRatesApp.Models.RatesChooseDate;

namespace ExchangeRatesApp.Client.Services
{
    public interface ICurrencyService
    {
        Task<List<Rate>> GetAllRatesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        Task<ExchangeRatesSeries> GetLastXCurrencies(string code, int topCount);
        Task<ExchangeRatesSeries> TryGetRatesFromTable(HttpClient httpClient, string table, string code, int topCount);
        Task<ExchangeRatesSeries> GetExchangeRatesOnDate(string code, DateTime date);
        Task<ExchangeRatesSeries> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate);
        Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table);
        Task<Rate?> GetCurrencyByCode(string code);
        Rate? FindCurrencyByCode(IEnumerable<CurrencyRates> currencies, string code);
        void HandleApiError();
    }
}

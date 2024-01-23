using ExchangeRatesApp.Models;

namespace ExchangeRatesApp.Client.Services
{
    public interface ICurrencyService
    {
        //Task<List<Rate>> GetAllRatesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        //Task<CurrencyRates> GetLastXCurrencies(string code, int topCount);
        //Task<CurrencyRates> TryGetRatesFromTable(HttpClient httpClient, string table, string code, int topCount);
        Task<CurrencyRates> GetExchangeRatesOnDate(string code, DateTime date);
        Task<CurrencyRates> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate);
        Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table);
        Task<Rate?> GetCurrencyByCode(string code);
        Rate? FindCurrencyByCode(IEnumerable<CurrencyRates> currencies, string code);
        void HandleApiError();
    }
}

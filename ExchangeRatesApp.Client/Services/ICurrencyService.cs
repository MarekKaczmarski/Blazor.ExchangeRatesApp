using ExchangeRatesApp.Models;

namespace ExchangeRatesApp.Client.Services
{
    public interface ICurrencyService
    {
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables();
        Task<CurrencyRates> GetExchangeRatesOnDate(string code, DateTime date);
        Task<CurrencyRates> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate);
        Task<string?> GetTable(string code, HttpClient httpClient);
        Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table);
    }
}

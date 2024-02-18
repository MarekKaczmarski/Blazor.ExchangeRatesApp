using ExchangeRatesApp.Models;

namespace ExchangeRatesApp.Client.Data
{
    public interface ICurrencyRepository
    {
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrenciesFromAllTablesWithPLN();
        Task<CurrencyRates> GetExchangeRatesOnDate(string code, DateTime date);
        Task<CurrencyRates> GetExchangeRatesInRange(string code, DateTime startDate, DateTime endDate);
        Task<string?> GetTable(string code, HttpClient httpClient);
        Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table);
    }
}

using CurrencyApplication.Models;

namespace CurrencyApplication.Client.Data
{
    public interface ICurrencyRepository
    {
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        Task<List<CurrencyRates>> GetAllCurrenciesFromTables();
        Task<List<CurrencyRates>> GetAllCurrenciesFromTablesWithPLN();
        Task<CurrencyRates> GetCurrencyRatesOnDate(string code, DateTime date);
        Task<CurrencyRates> GetCurrencyRatesInRange(string code, DateTime startDate, DateTime endDate);
        Task<string?> GetTable(string code, HttpClient httpClient);
        Task<string> TryGetRatesFromTable(string code, HttpClient httpClient, string table);
    }
}

using ExchangeRatesApp.Models;

namespace ExchangeRatesApp.Client.Services
{
    public interface ICurrencyService
    {
        Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        Task<Rate?> GetCurrencyByCode(string code);
        Task<Rate?> GetCurrencyByCode(string table, string code);
    }
}

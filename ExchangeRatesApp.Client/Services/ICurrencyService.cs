using ExchangeRatesApp.Models;
using ExchangeRatesApp.Models.RatesChooseDate;

namespace ExchangeRatesApp.Client.Services
{
    public interface ICurrencyService
    {
        Task<List<Rate>> GetAllRatesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrenciesFromAllTables();
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        Task<Rate?> GetCurrencyByCode(string code);
        Rate? FindCurrencyByCode(IEnumerable<CurrencyRates> currencies, string code);
    }
}

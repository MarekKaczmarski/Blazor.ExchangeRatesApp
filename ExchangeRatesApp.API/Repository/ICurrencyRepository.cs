using ExchangeRatesApp.API.Entities;

namespace ExchangeRatesApp.API.Repository
{
    public interface ICurrencyRepository
    {
        Task<List<CurrencyRates>> GetAllCurrencies(string table);
        Task<Rate?> GetCurrencyByCode(string code);
        Task<Rate?> GetCurrencyByCode(string table, string code);
        
    }
}

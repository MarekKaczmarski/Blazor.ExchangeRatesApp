using ExchangeRatesApp.API.Entities;

namespace ExchangeRatesApp.API.Repository
{
    public interface ICurrencyRepository
    {
        Task<List<CurrencyRates>> GetAllCurrencies();
        Task<Rate?> GetCurrencyByCode(string code);
    }
}

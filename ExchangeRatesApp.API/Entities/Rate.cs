namespace ExchangeRatesApp.API.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public double Mid { get; set; }
        public int CurrencyRatesId { get; set; }
        public CurrencyRates? CurrencyRates { get; set; }
    }
}

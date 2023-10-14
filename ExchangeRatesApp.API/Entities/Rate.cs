namespace ExchangeRatesApp.API.Entities
{
    public class Rate
    {
        public string Currency { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public double Mid { get; set; }
    }
}

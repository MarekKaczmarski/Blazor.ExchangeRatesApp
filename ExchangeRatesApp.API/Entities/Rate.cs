namespace ExchangeRatesApp.API.Entities
{
    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public double mid { get; set; }
    }
}

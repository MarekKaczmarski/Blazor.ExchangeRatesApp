namespace ExchangeRatesApp.API.Entities
{
    public class CurrencyRates
    {
            public string table { get; set; }
            public string no { get; set; }
            public string effectiveDate { get; set; }
            public List<Rate> rates { get; set; }
    }
}

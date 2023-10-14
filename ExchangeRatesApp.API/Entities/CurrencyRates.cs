namespace ExchangeRatesApp.API.Entities
{
    public class CurrencyRates
    {
            public string Table { get; set; } = string.Empty;
            public string No { get; set; } = string.Empty;
            public string EffectiveDate { get; set; } = string.Empty;
            public List<Rate> Rates { get; set; } = new List<Rate>();
    }
}

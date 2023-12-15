namespace ExchangeRatesApp.Client.Helpers
{
    public static class CurrencyHelper
    {
        public static string GetFlagImageName(string currencyCode)
        {
            return $"{currencyCode.ToLower()}.png";
        }
    }
}

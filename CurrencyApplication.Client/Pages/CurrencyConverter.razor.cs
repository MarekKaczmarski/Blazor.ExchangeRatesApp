using CurrencyApplication.Models;

namespace CurrencyApplication.Client.Pages
{
    public partial class CurrencyConverter
    {
        private List<CurrencyRates>? currencies;
        private IEnumerable<Rate>? uniqueCurrencies;
        private string sourceCurrencyCode = string.Empty;
        private string targetCurrencyCode = string.Empty;
        private string result = string.Empty;
        private bool showResult = false;

        private decimal amountToConvert = 1.0m;

        protected override async Task OnInitializedAsync()
        {
            currencies = await CurrencyService.GetAllCurrenciesFromTablesWithPLN();
            uniqueCurrencies = currencies
                .SelectMany(cr => cr.Rates)
                .GroupBy(rate => rate.Code)
                .Select(group => group.First());
        }

        private void ConvertCurrency()
        {
            showResult = true;
            if (currencies != null && sourceCurrencyCode != null && targetCurrencyCode != null)
            {
                var sourceCurrency = currencies
                    .SelectMany(cr => cr.Rates)
                    .FirstOrDefault(rate => rate.Code == sourceCurrencyCode);

                var targetCurrency = currencies
                    .SelectMany(cr => cr.Rates)
                    .FirstOrDefault(rate => rate.Code == targetCurrencyCode);

                if (sourceCurrency != null && amountToConvert != 0 && targetCurrency != null)
                {
                    var convertedAmount = CalculateConvertedAmount(sourceCurrency, targetCurrency);
                    result = $"{amountToConvert} {sourceCurrencyCode} = {convertedAmount} {targetCurrencyCode}";
                }
            }
        }

        private decimal CalculateConvertedAmount(Rate sourceCurrency, Rate targetCurrency)
        {
            decimal convertedAmount = (amountToConvert * sourceCurrency.Mid) / targetCurrency.Mid;
            convertedAmount = Math.Round(convertedAmount, 2);

            return convertedAmount;
        }

        private void SwapCurrencies()
        {
            string temp = sourceCurrencyCode;
            sourceCurrencyCode = targetCurrencyCode;
            targetCurrencyCode = temp;
        }

        private void ClearResult()
        {
            result = string.Empty;
            showResult = false;
        }
    }
}

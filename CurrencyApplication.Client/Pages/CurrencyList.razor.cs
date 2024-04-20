using CurrencyApplication.Models;

namespace CurrencyApplication.Client.Pages
{
    public partial class CurrencyList
    {
        private List<CurrencyRates> CurrencyRates = new List<CurrencyRates>();
        private List<Rate> Rates = new List<Rate>();

        private string searchString = "";

        protected override async Task OnInitializedAsync()
        {
            CurrencyRates = await CurrencyService.GetAllCurrenciesFromTables();
            Rates = CurrencyRates.SelectMany(currencyRates => currencyRates.Rates).ToList();
        }

        private bool FilterFunc1(Rate element) => FilterFunc(element, searchString);

        private bool FilterFunc(Rate element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Currency.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Mid}".Contains(searchString))
                return true;
            return false;
        }
    }
}

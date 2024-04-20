using CurrencyApplication.Models;

namespace CurrencyApplication.Client.Pages
{
    public partial class CurrencyProfit
    {
        private List<CurrencyRates> CurrencyRates = new List<CurrencyRates>();
        private List<Rate> Rates = new List<Rate>();

        private IEnumerable<string> selectedRates { get; set; } = new HashSet<string>();
        private Dictionary<string, DateTime?> selectedCurrencyPurchaseDate = new Dictionary<string, DateTime?>();
        private Dictionary<string, decimal> selectedCurrencyPurchaseValue = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> profits = new Dictionary<string, decimal>();

        private decimal profit = 0;
        private bool profitCalculated = false;

        private string errorMessage = "";

        protected override async Task OnInitializedAsync()
        {
            CurrencyRates = await CurrencyService.GetAllCurrenciesFromTables();
            Rates = CurrencyRates.SelectMany(currencyRates => currencyRates.Rates).ToList();
        }

        private async Task CalculateProfit()
        {
            CurrencyRates exchangeRates;

            errorMessage = string.Empty;
            profitCalculated = false;
            profits.Clear();
            profit = 0;

            if (selectedRates.Any())
            {
                foreach (var selectedCurrency in selectedRates)
                {
                    var purchaseDate = selectedCurrencyPurchaseDate[selectedCurrency];
                    var purchaseValue = selectedCurrencyPurchaseValue[selectedCurrency];

                    if (purchaseDate.HasValue)
                    {
                        var currentRate = Rates.FirstOrDefault(r => r.Code == selectedCurrency)?.Mid;
                        if (currentRate == null)
                        {
                            continue;
                        }

                        try
                        {
                            exchangeRates = await CurrencyService.GetCurrencyRatesOnDate(selectedCurrency, purchaseDate.Value);

                            if (exchangeRates == null || exchangeRates.Rates == null || !exchangeRates.Rates.Any())
                            {
                                return;
                            }
                        }
                        catch (HttpRequestException)
                        {
                            errorMessage = "Brak kursów dla podanej daty";
                            continue;
                        }

                        var purchaseRate = exchangeRates?.Rates.FirstOrDefault()?.Mid;
                        if (purchaseRate == null)
                        {
                            continue;
                        }

                        var currentPurchaseValue = purchaseValue / purchaseRate.Value * currentRate.Value;
                        var profitLoss = currentPurchaseValue - purchaseValue;

                        if (profits.ContainsKey(selectedCurrency))
                        {
                            profits[selectedCurrency] += profitLoss;
                        }
                        else
                        {
                            profits[selectedCurrency] = profitLoss;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                profitCalculated = true;
            }
            else
            {
                //brak wybranych walut
            }

        }

        private decimal totalProfit => Math.Round(profits.Values.Sum(), 2);
        private bool FutureDate(DateTime dt)
        {
            return CheckFutureDateWithNoWeekends(dt);
        }

        private bool CheckFutureDateWithNoWeekends(DateTime dt)
        {
            if (dt > DateTime.Today)
            {
                return true;
            }

            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            return false;
        }
    }
}

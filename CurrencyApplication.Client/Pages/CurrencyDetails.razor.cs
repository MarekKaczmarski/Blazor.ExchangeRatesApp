using ApexCharts;
using CurrencyApplication.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CurrencyApplication.Client.Pages
{
    public partial class CurrencyDetails
    {
        [Parameter]
        public string? Code { get; set; }

        private CurrencyRates? Rate { get; set; }

        private DateTime currentDate;
        private decimal currentMid;

        private DateTime? date { get; set; }
        private ApexChartOptions<Rate>? options;
        private ApexChart<Rate>? chart;

        private decimal YTDPercentage { get; set; }
        private decimal Change { get; set; }

        private MudDateRangePicker? _picker;
        private DateRange? _dateRange;
        private bool _autoClose;

        private string? errorMessage;

        protected override async Task OnInitializedAsync()
        {
            await LoadRatesForYTD();
            await LoadRatesForPeriod(1);
            await LoadChangesForRates();
            await base.OnInitializedAsync();
        }

        private async Task LoadRatesForPeriod(int months)
        {
            var startDate = DateTime.Now.AddMonths(-months);
            var endDate = DateTime.Now;
            await LoadRates(startDate, endDate);
        }

        private async Task LoadRatesForCustomPeriod()
        {
            if (_dateRange != null && _dateRange.Start.HasValue && _dateRange.End.HasValue && _picker != null)
            {
                var startDate = _dateRange.Start.Value;
                var endDate = _dateRange.End.Value;
                await LoadRates(startDate, endDate);
            }
        }

        private async Task LoadRates(DateTime startDate, DateTime endDate)
        {
            if (CurrencyService == null)
            {
                return;
            }

            try
            {
                Rate = await CurrencyService.GetCurrencyRatesInRange(Code, startDate, endDate);

                if (Rate == null)
                {
                    return;
                }

                if (currentMid == 0 && currentDate == DateTime.MinValue && Rate.Rates.Any())
                {
                    currentMid = Rate.Rates[^1].Mid;
                    currentDate = Rate.Rates[^1].EffectiveDate;
                }

                options = new ApexChartOptions<Rate>
                {
                    Chart = new Chart
                    {
                        Height = 500,
                        Width = "100%",
                        Toolbar = new Toolbar
                        {
                            Show = false
                        },
                    },

                };
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "Brak kursów dla podanego przedziału czasowego";
            }
        }

        private async Task HandleButtonClick(int months)
        {
            await LoadRatesForPeriod(months);
            StateHasChanged();
            await chart.UpdateSeriesAsync(true);
        }

        private async Task HandleCustomButtonClick()
        {
            await LoadRatesForCustomPeriod();
            StateHasChanged();
            await chart.UpdateSeriesAsync(true);
        }

        string FirstLetterToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpperInvariant(input[0]) + input.Substring(1);
        }

        private async Task LoadRatesForYTD()
        {
            var startDateYTD = new DateTime(DateTime.Now.Year, 1, 1);
            var endDateYTD = DateTime.Now;

            var ratesForYTD = await CurrencyService.GetCurrencyRatesInRange(Code, startDateYTD, endDateYTD);

            if (ratesForYTD == null || ratesForYTD.Rates == null || ratesForYTD.Rates.Count == 0)
            {
                YTDPercentage = 0;
                return;
            }

            decimal initialValue = ratesForYTD.Rates.First().Mid;
            decimal finalValue = ratesForYTD.Rates.Last().Mid;

            decimal percentageChange = ((finalValue / initialValue) - 1) * 100;

            YTDPercentage = percentageChange;
        }

        decimal CalculateYTD(List<Rate> rates)
        {
            if (rates == null || rates.Count == 0)
            {
                return 0;
            }

            rates = rates.OrderBy(rate => rate.EffectiveDate).ToList();

            decimal firstMid = rates.First().Mid;
            decimal lastMid = rates.Last().Mid;

            decimal ytdPercentage = ((lastMid / firstMid) - 1) * 100;

            string formattedResult = ytdPercentage.ToString("0.00");

            return decimal.Parse(formattedResult);
        }

        private async Task LoadChangesForRates()
        {
            var startDateYTD = new DateTime(DateTime.Now.Year, 1, 1);
            var endDateYTD = DateTime.Now;

            var rates = await CurrencyService.GetCurrencyRatesInRange(Code, startDateYTD, endDateYTD);

            if (rates == null || rates.Rates == null || rates.Rates.Count == 0)
            {
                return;
            }

            decimal firstValue = rates.Rates.First().Mid;
            decimal secondValue = rates.Rates.Skip(1).First().Mid;

            decimal rateChange = firstValue - secondValue;

            Change = rateChange;
        }

        private void CloseErrorNotification()
        {
            errorMessage = string.Empty;
        }

        private bool FutureDate(DateTime dt)
        {
            DateTime today = DateTime.Now.Date;
            return dt.Date > today;
        }
    }
}

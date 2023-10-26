using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Models
{
    public class CurrencyRates
    {
        public int Id { get; set; }
        public string Table { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty;
        public string EffectiveDate { get; set; } = string.Empty;
        public List<Rate> Rates { get; set; } = new List<Rate>();
    }
}

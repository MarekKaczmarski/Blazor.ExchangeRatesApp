using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyApplication.Models
{
    public class CurrencyRates
    {
        public string Table { get; set; } = string.Empty;
        public string? Currency { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public string? No { get; set; } = string.Empty;

        [Display(Name = "TradingDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string? TradingDate { get; set; } = string.Empty;

        [Display(Name = "EffectiveDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EffectiveDate { get; set; }
        public List<Rate> Rates { get; set; } = new List<Rate>();
    }
}

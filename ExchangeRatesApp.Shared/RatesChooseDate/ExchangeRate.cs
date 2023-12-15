using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Models.RatesChooseDate
{
    public class ExchangeRate
    {
        public string No { get; set; }

        [Display(Name = "EffectiveDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }
}

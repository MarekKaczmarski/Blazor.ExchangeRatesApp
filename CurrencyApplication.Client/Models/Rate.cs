using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyApplication.Models
{
    public class Rate
    {
        public string? Currency { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public decimal Mid { get; set; }
        public string? No { get; set; } = string.Empty;
        [Display(Name = "EffectiveDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Models
{
    public class Rate
    {
        public string Currency { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public double Mid { get; set; }
    }
}

using ExchangeRatesApp.API.Entities;
using ExchangeRatesApp.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace ExchangeRatesApp.API.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyRatesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyRatesController(HttpClient httpClient, ICurrencyRepository currencyRepository)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.nbp.pl/");
            _currencyRepository = currencyRepository;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetCurrencyRates()
        //{
        //    var response = await _httpClient.GetAsync("api/exchangerates/tables/a/?format=json");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = await response.Content.ReadFromJsonAsync<List<CurrencyRates>>();
        //        return Ok(data);
        //    }

        //    return BadRequest("Failed to retrieve currency rates.");
        //}

        [HttpGet("all-currencies/{table}")]
        public async Task<IActionResult> GetAllCurrencies(string table)
        {
            try
            {
                var currencyRates = await _currencyRepository.GetAllCurrencies(table);
                return Ok(currencyRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }

        [HttpGet("currency-by-code/{code}")]
        public async Task<IActionResult> GetCurrencyByCode(string code)
        {
            try
            {
                var specificCurrency = await _currencyRepository.GetCurrencyByCode(code);

                if (specificCurrency != null)
                {
                    return Ok(specificCurrency);
                }
                else
                {
                    return NotFound($"Currency with code {code} not found in any table.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }

        [HttpGet("currency/{table}/{code}")]
        public async Task<IActionResult> GetCurrencyByCode(string table, string code)
        {
            try
            {
                var specificCurrency = await _currencyRepository.GetCurrencyByCode(table, code);

                if (specificCurrency != null)
                {
                    return Ok(specificCurrency);
                }
                else
                {
                    return NotFound($"Currency with code {code} in {table} table not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }
    }
}

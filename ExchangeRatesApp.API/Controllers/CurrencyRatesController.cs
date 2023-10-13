using ExchangeRatesApp.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRatesApp.API.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class CurrencyRatesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CurrencyRatesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.nbp.pl/");
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencyRates()
        {
            var response = await _httpClient.GetAsync("api/exchangerates/tables/a/?format=json");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<CurrencyRates>>();
                return Ok(data);
            }

            return BadRequest("Failed to retrieve currency rates.");
        }
    }
}

using Bunit;
using ExchangeRatesApp.Client.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Client.Services;
using ExchangeRatesApp.Models;
using Moq;

namespace ExchangeRatesApp.Tests
{
    public class CurrencyServiceTests
    {
        private readonly Mock<CurrencyDataService> _currencyDataServiceMock;
        private readonly ICurrencyService _currencyService;

        public CurrencyServiceTests()
        {
            _currencyDataServiceMock = new Mock<CurrencyDataService>();
            _currencyService = new CurrencyService(_currencyDataServiceMock.Object);
        }

        [Fact]
        public async Task GetAllCurrencies_ReturnsCurrencyRates_WhenCalled()
        {
            // Arrange
            var currencyRates = new List<CurrencyRates> { new CurrencyRates { Table = "A", Currency = "dolar amerykański", Code = "USD" } };
            _currencyDataServiceMock.Setup(_ => _.GetAllCurrencies(It.IsAny<string>())).ReturnsAsync(currencyRates);

            // Act
            var result = await _currencyService.GetAllCurrencies("A");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("A", result[0].Table);
            Assert.Equal("dolar amerykański", result[0].Currency);
            Assert.Equal("USD", result[0].Code);
        }
    }
}

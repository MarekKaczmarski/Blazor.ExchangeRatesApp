using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Client.Services;
using ExchangeRatesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Net.Http.Json;
using System.Net;

namespace ExchangeRatesApp.Tests
{
    public class CurrencyDataServiceTests
    {
        [Fact]
        public async Task GetAllCurrenciesFromAllTables_ReturnsUniqueCurrencies()
        {
            // Arrange
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClientMock = new Mock<HttpClient>();
            var currencyRatesList = new List<CurrencyRates>
            {

            };

            httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(currencyRatesList)
                });

            httpClientFactoryMock.Setup(f => f.CreateClient("NBPClient"))
                .Returns(httpClientMock.Object);

            var currencyDataService = new CurrencyDataService(httpClientFactoryMock.Object);

            // Act
            var result = await currencyDataService.GetAllCurrenciesFromAllTables();

            // Assert
        }

        [Fact]
        public async Task GetAllCurrenciesFromAllTables_HandlesHttpClientError()
        {
            // Arrange
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClientMock = new Mock<HttpClient>();

            httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            httpClientFactoryMock.Setup(f => f.CreateClient("NBPClient"))
                .Returns(httpClientMock.Object);

            var currencyDataService = new CurrencyDataService(httpClientFactoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => currencyDataService.GetAllCurrenciesFromAllTables());
        }
    }
}

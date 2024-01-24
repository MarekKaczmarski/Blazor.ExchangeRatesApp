using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Tests
{
    public class CurrencyDataServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly CurrencyDataService _currencyDataService;

        public CurrencyDataServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _currencyDataService = new CurrencyDataService(_httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task GetAllCurrencies_ReturnsCurrencyRates_WhenResponseIsSuccess()
        {
            // Arrange
            var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK, "[{\"table\":\"A\",\"currency\":\"dolar amerykański\",\"code\":\"USD\",\"rates\":[{\"no\":\"A/2024/01/24\",\"effectiveDate\":\"2024-01-24\",\"mid\":3.72}]}]"));
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _currencyDataService.GetAllCurrencies("A");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("A", result[0].Table);
            Assert.Equal("dolar amerykański", result[0].Currency);
            Assert.Equal("USD", result[0].Code);
            Assert.Single(result[0].Rates);
            Assert.Equal(3.72m, result[0].Rates[0].Mid);
        }

        [Fact]
        public async Task GetAllCurrencies_ThrowsException_WhenResponseIsNotSuccess()
        {
            // Arrange
            var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.BadRequest, ""));
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _currencyDataService.GetAllCurrencies("A"));
        }

    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _httpStatusCode;
        private readonly string _content;

        public MockHttpMessageHandler(HttpStatusCode httpStatusCode, string content)
        {
            _httpStatusCode = httpStatusCode;
            _content = content;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://api.nbp.pl/") };
            request.RequestUri = new Uri(httpClient.BaseAddress, request.RequestUri.PathAndQuery);
            return new HttpResponseMessage
            {
                StatusCode = _httpStatusCode,
                Content = new StringContent(_content),
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

public class CurrencyRepositoryTests
{
    [Fact]
    public async Task GetAllCurrencies_ReturnsCurrencies()
    {
        // Arrange
        var currencyRepository = new Mock<ICurrencyRepository>();
        var expectedCurrencies = new List<CurrencyRates> { /* mock your expected data */ };
        currencyRepository.Setup(repo => repo.GetAllCurrencies(It.IsAny<string>()))
                          .ReturnsAsync(expectedCurrencies);

        // Act
        var result = await currencyRepository.Object.GetAllCurrencies("table");

        // Assert
        Assert.Equal(expectedCurrencies, result);
    }

    [Fact]
    public async Task GetAllCurrencies_SuccessfulRequest_ReturnsCurrencies()
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpClientMock = new Mock<HttpClient>();
        var expectedCurrencies = new List<CurrencyRates> { /* mock your expected data */ };

        httpClientFactoryMock.Setup(factory => factory.CreateClient("NBPClient")).Returns(httpClientMock.Object);
        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                      .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                      {
                          Content = JsonContent.Create(expectedCurrencies)
                      });

        var currencyRepository = new CurrencyRepository(httpClientFactoryMock.Object);

        // Act
        var result = await currencyRepository.GetAllCurrencies("table");

        // Assert
        Assert.Equal(expectedCurrencies, result);
    }

    [Fact]
    public async Task GetAllCurrencies_UnsuccessfulRequest_ReturnsEmptyList()
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpClientMock = new Mock<HttpClient>();

        httpClientFactoryMock.Setup(factory => factory.CreateClient("NBPClient")).Returns(httpClientMock.Object);
        httpClientMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                      .ThrowsAsync(new HttpRequestException("Simulated error"));

        var currencyRepository = new CurrencyRepository(httpClientFactoryMock.Object);

        // Act
        var result = await currencyRepository.GetAllCurrencies("table");

        // Assert
        Assert.Empty(result);
    }
}

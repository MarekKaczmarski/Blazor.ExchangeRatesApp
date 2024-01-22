using ApexCharts;
using Bunit;
using ExchangeRatesApp.Client.Pages;
using ExchangeRatesApp.Client.Services;
using ExchangeRatesApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Tests
{
    public class CurrencyListTest : TestContext
    {
        [Fact]
        public async Task RendersLoadingMessageWhileFetchingRates()
        {
            // Arrange
            var currencyServiceMock = new Mock<ICurrencyService>();
            currencyServiceMock.Setup(s => s.GetAllRatesFromAllTables()).ReturnsAsync(new List<Rate>());

            var keyInterceptorFactoryMock = new Mock<IKeyInterceptorFactory>();

            Services.AddSingleton<ICurrencyService>(currencyServiceMock.Object);
            Services.AddSingleton<IKeyInterceptorFactory>(keyInterceptorFactoryMock.Object);

            var scrollManagerMock = new Mock<IScrollManager>();
            Services.AddSingleton<IScrollManager>(scrollManagerMock.Object);

            var popoverServiceMock = new Mock<IPopoverService>();
            Services.AddSingleton<IPopoverService>(popoverServiceMock.Object);

            var mudPopoverServiceMock = new Mock<IMudPopoverService>();
            Services.AddSingleton<IMudPopoverService>(mudPopoverServiceMock.Object);


            // Act
            var cut = RenderComponent<CurrencyList>();

            // Assert
            cut.Find(".mt-4").TextContent.ShouldBe("Ładowanie listy walut...");
            //Xunit.Assert.Equal("Ładowanie listy walut...", cut.Find(".mt-4").TextContent);
        }

        [Fact]
        public async Task RendersCurrencyListAfterFetchingRates()
        {
            // Arrange
            var currencyServiceMock = new Mock<ICurrencyService>();
            currencyServiceMock.Setup(s => s.GetAllRatesFromAllTables()).ReturnsAsync(new List<Rate>
            {
                new Rate { Currency = "Euro", Code = "EUR", Mid = 4.2m },
                // Add more sample rates as needed
            });

            var serviceContext = new TestServiceProvider();
            serviceContext.AddSingleton<ICurrencyService>(currencyServiceMock.Object);

            // Act
            var cut = RenderComponent<CurrencyList>();

            // Wait for the component to finish rendering after rates are fetched
            cut.WaitForState(() => cut.FindAll("tr").Count > 1);

            // Assert
            cut.Find(".mt-4").TextContent.ShouldBeEmpty(); // Loading message should be hidden
            cut.FindAll("tr").ShouldNotBeEmpty(); // Rows should be present in the table
        }

        [Fact]
        public void FiltersCurrencyListBasedOnSearchString()
        {
            // Arrange
            var currencyServiceMock = new Mock<ICurrencyService>();
            currencyServiceMock.Setup(s => s.GetAllRatesFromAllTables()).ReturnsAsync(new List<Rate>
            {
                new Rate { Currency = "Euro", Code = "EUR", Mid = 4.2m },
                new Rate { Currency = "US Dollar", Code = "USD", Mid = 3.5m },
                // Add more sample rates as needed
            });

            var testContext = new TestContext();

            // Emuluj IJSRuntime
            var jsRuntimeMock = new Mock<IJSRuntime>();
            jsRuntimeMock.Setup(j => j.InvokeAsync<object>(It.IsAny<string>(), It.IsAny<object[]>()))
                         .ReturnsAsync(new ValueTask<object>(new object()));

            //MudBlazor services
            var keyInterceptorFactoryMock = new Mock<IKeyInterceptorFactory>();
            testContext.Services.AddSingleton<IKeyInterceptorFactory>(keyInterceptorFactoryMock.Object);

            var scrollManagerMock = new Mock<IScrollManager>();
            testContext.Services.AddSingleton<IScrollManager>(scrollManagerMock.Object);

            var popoverServiceMock = new Mock<IPopoverService>();
            testContext.Services.AddSingleton<IPopoverService>(popoverServiceMock.Object);

            testContext.Services.AddSingleton<IJSRuntime>(jsRuntimeMock.Object);

            testContext.Services.AddSingleton<ICurrencyService>(currencyServiceMock.Object);

            //var cut = RenderComponent<CurrencyList>(parameters => parameters.AddTestServices(serviceContext));

            // Act
            var cut = testContext.RenderComponent<CurrencyList>();

            // Act
            cut.Find("input").Change("Euro");

            // Assert
            cut.WaitForState(() => cut.FindAll("tr").Count == 2);

            Xunit.Assert.Equal("Euro", cut.Find("tr").TextContent);
            Xunit.Assert.Equal("EUR", cut.Find("tr td:nth-child(2)").TextContent);
            Xunit.Assert.Equal("4.2", cut.Find("tr td:nth-child(3)").TextContent);
        }
    }
}

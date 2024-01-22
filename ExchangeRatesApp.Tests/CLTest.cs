using Bunit;
using ExchangeRatesApp.Client.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Bunit;

namespace ExchangeRatesApp.Tests
{
    public class CLTest : TestContext
    {
        [Fact]
        public void ShouldRenderComponent()
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var component = ctx.RenderComponent<CurrencyList>();

            // Assert
            Assert.NotNull(component);
            // Add more assertions as needed
        }
    }
}

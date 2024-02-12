using ExchangeRatesApp.Client;
using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Extensions.Logging;
using MudBlazor.Services;
using MudExtensions.Services;
using Blazored.LocalStorage;
using ApexCharts;
using ExchangeRatesApp.Client.Helpers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

SelfLog.Enable(m => Console.Error.WriteLine(m));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.BrowserConsole()
    .CreateLogger();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("NBPClient", client =>
{
    client.BaseAddress = new Uri("https://api.nbp.pl/");
});
builder.Services.AddTransient<CurrencyDataService>();
builder.Services.AddTransient<ICurrencyService, CurrencyService>();

await builder.Build().RunAsync();

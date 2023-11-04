using ExchangeRatesApp.Client;
using ExchangeRatesApp.Client.Data;
using ExchangeRatesApp.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

SelfLog.Enable(m => Console.Error.WriteLine(m));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.BrowserConsole()
    .CreateLogger();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("NBPClient", client =>
{
    client.BaseAddress = new Uri("https://api.nbp.pl/");
});
builder.Services.AddScoped<CurrencyDataService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

await builder.Build().RunAsync();

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using OlxParser.WEB;
using OlxParser.WEB.Services;
using OlxParser.WEB.Services.Contracts;
using OlxParser.WEB.StateContainers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var localhostUri = "https://localhost:7063";
var azureUri = "https://flatnotifierapi.azurewebsites.net";
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(azureUri) });
builder.Services.AddScoped<IHtmlParseService, HtmlParseService>();
builder.Services.AddScoped<FlatStateContainer>();

await builder.Build().RunAsync();

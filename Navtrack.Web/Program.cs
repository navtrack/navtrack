using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Navtrack.Web.Config;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSentry();

builder.Services.AddSpaStaticFiles(spaStaticFilesOptions => { spaStaticFilesOptions.RootPath = "ClientApp/build"; });


WebApplication app = builder.Build();

app.UseSentryTracing();
app.UseSpaStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        spa.UseReactDevelopmentServer("start");
    }
});

if (!app.Environment.IsDevelopment())
{
    app.MapGet("/config.json", () => JsonSerializer.Serialize(new Config
    {
        ApiUrl = Environment.GetEnvironmentVariable(ConfigVariables.ApiUrl)
    }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
}

app.Run();
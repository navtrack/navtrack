using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

app.Run();
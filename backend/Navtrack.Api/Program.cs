using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Api.Hubs;
using Navtrack.Api.Services;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

const string defaultCorsPolicy = "defaultCorsPolicy";

Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Bootstrapper.ConfigureServices<Program>(builder.Services);

builder.WebHost.UseSentry();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(defaultCorsPolicy, builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
});

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<AuthorizeActionFilter>();
        options.Filters.Add<ModelStateMappingActionFilter>();
    })
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddSignalR().AddHubOptions<AssetsHub>(options => { options.EnableDetailedErrors = true; });

builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
    .AddInMemoryApiScopes(IdentityServerConfig.GetScopes());

builder.Services.AddLocalApiAuthentication();
builder.Services.AddLogging();

builder.Services.AddOptions<MongoOptions>().Bind(builder.Configuration.GetSection(nameof(MongoOptions)));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Navtrack API"); });
}
else
{
    app.UseHsts();
}

app.UseCors(defaultCorsPolicy);
app.UseRouting();
app.UseSentryTracing();

app.UseSignalRQueryStringAuthentication();
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapHub<AssetsHub>(ApiConstants.HubUrl("assets"));

app.MapControllers();

app.Run();

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}
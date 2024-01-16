using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.Api.Services.Mappers.Common;
using Navtrack.Api.Shared.Hubs;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Shared;

public abstract class BaseApiProgram<T>
{
    public static void Main(string[] args, Assembly assembly, BaseProgramOptions? baseProgramOptions = null)
    {
        const string defaultCorsPolicy = "defaultCorsPolicy";

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddCustomServices<T>();
        
        builder.WebHost.UseSentry();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument(c => { c.Title = assembly.GetName().Name; });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(defaultCorsPolicy, policyBuilder =>
            {
                policyBuilder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
        });

        builder.Services.AddControllers(options =>
            {
                options.Filters.Add<AuthorizeActionFilter>();
                baseProgramOptions?.Filters?.ForEach(x => options.Filters.Add(x));
            })
            .ConfigureApplicationPartManager(x =>
            {
                IApplicationFeatureProvider applicationFeatureProvider =
                    x.FeatureProviders.First(y => y.GetType() == typeof(ControllerFeatureProvider));

                x.FeatureProviders[x.FeatureProviders.IndexOf(applicationFeatureProvider)] =
                    new CustomControllerFeatureProvider();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    ValidationProblemDetails problemDetails = new(context.ModelState);
                    ErrorModel errorModel = ErrorModelMapper.Map(problemDetails);

                    return new BadRequestObjectResult(errorModel);
                };
            })
            .AddJsonOptions(options => ConfigureJsonOptions(options.JsonSerializerOptions));

        builder.Services.Configure<JsonSerializerOptions>(ConfigureJsonOptions);

        builder.Services.AddSignalR().AddHubOptions<AssetsHub>(options => { options.EnableDetailedErrors = true; });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddIdentityServer()
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityServerConfig.GetScopes());

        builder.Services.AddLocalApiAuthentication();
        builder.Services.AddLogging();

        builder.Services.AddOptions<MongoOptions>().Bind(builder.Configuration.GetSection(nameof(MongoOptions)));
        builder.Services.AddSingleton<IClientErrorFactory, CustomClientErrorFactory>();

        baseProgramOptions?.ConfigureServices?.Invoke(builder);

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi();
        }
        else
        {
            app.UseHsts();
        }

        app.UseCors(defaultCorsPolicy);
        app.UseRouting();
        app.UseSentryTracing();

        // app.UseSignalRQueryStringAuthentication();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseIdentityServer();

        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();
        app.MapHub<AssetsHub>(ApiConstants.HubUrl("assets"));

        app.Run();
    }

    private static void ConfigureJsonOptions(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new JsonStringEnumConverter());
    }
}
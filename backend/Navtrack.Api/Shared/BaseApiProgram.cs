using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.IdentityServer;
using Navtrack.Api.Services.Common.Mappers;
using Navtrack.Database.Model;
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
                options.Filters.Add<AuthorizeOrganizationActionFilter>();
                options.Filters.Add<AuthorizeTeamActionFilter>();
                options.Filters.Add<AuthorizeAssetActionFilter>();

                baseProgramOptions?.Filters?.ForEach(x => options.Filters.Add(x));
            })
            .ConfigureApplicationPartManager(applicationPartManager =>
            {
                IApplicationFeatureProvider applicationFeatureProvider =
                    applicationPartManager.FeatureProviders.First(y =>
                        y.GetType() == typeof(ControllerFeatureProvider));

                applicationPartManager.FeatureProviders[
                        applicationPartManager.FeatureProviders.IndexOf(applicationFeatureProvider)] =
                    new CustomControllerFeatureProvider();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    ValidationProblemDetails problemDetails = new(context.ModelState);
                    ErrorModel error = ErrorMapper.Map(problemDetails);

                    return new BadRequestObjectResult(error);
                };
            })
            .AddJsonOptions(options => ConfigureJsonOptions(options.JsonSerializerOptions));

        builder.Services.Configure<JsonSerializerOptions>(ConfigureJsonOptions);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddIdentityServer()
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityServerConfig.GetScopes());

        builder.Services.AddLocalApiAuthentication();
        builder.Services.AddLogging();

        builder.Services.AddSingleton<IClientErrorFactory, CustomClientErrorFactory>();  
        
        baseProgramOptions?.ConfigureServices?.Invoke(builder);

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi(o =>
            {
                o.TagsSorter = "alpha";
                o.DocumentTitle = "Navtrack API";
            });
        }
        else
        {
            app.UseHsts();
        }

        app.UseCors(defaultCorsPolicy);
        app.UseRouting();

        // app.UseSignalRQueryStringAuthentication();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseIdentityServer();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<NavtrackContextMiddleware>();

        app.MapControllers();
        // app.MapHub<AssetsHub>(ApiConstants.HubUrl("assets"));

        if (app.Environment.IsProduction() && baseProgramOptions?.MigrateDatabase == true)
        {
            using IServiceScope scope = app.Services.CreateScope();
            DbContext db = scope.ServiceProvider.GetRequiredService<DbContext>();
            db.Database.Migrate();
        }
        
        app.Run();
    }

    private static void ConfigureJsonOptions(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new JsonStringEnumConverter());
    }
}
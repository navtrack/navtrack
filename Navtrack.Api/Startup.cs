using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Api.Hubs;
using Navtrack.Api.Services;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api;

public class Startup
{
    private const string DefaultCorsPolicy = "defaultCorsPolicy";
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicy, builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
        });

        services.AddControllers(options =>
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
        services.AddSignalR().AddHubOptions<AssetsHub>(options => { options.EnableDetailedErrors = true; });

        services.AddHttpContextAccessor();

        services.AddIdentityServer()
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityServerConfig.GetScopes());

        services.AddLocalApiAuthentication();

        services.AddSwaggerGen();
        services.AddLogging();

        services.AddOptions<MongoOptions>().Bind(Configuration.GetSection(nameof(MongoOptions)));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Navtrack API"); });
        }
        else
        {
            app.UseHsts();
        }

        app.UseCors(DefaultCorsPolicy);
        app.UseRouting();
        app.UseSentryTracing();

        app.UseSignalRQueryStringAuthentication();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseIdentityServer();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<AssetsHub>(ApiConstants.HubUrl("assets"));
        });
    }
}
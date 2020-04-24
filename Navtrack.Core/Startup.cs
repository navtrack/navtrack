using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.Api.Services.LetsEncrypt;

namespace Navtrack.Core
{
    public class Startup
    {
        private const string DefaultCorsPolicy = "defaultCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy, builder =>
                {
                    builder.WithOrigins(Configuration["FrontEndUrl"])
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            Assembly assembly = typeof(Api.Reference).Assembly;
            
            services.AddControllers()
                .AddApplicationPart(assembly)
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);;
            
            services.AddHttpContextAccessor();
            
            
            // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //     .AddCookie(options =>
            //     {
            //         options.Events = new CookieAuthenticationEvents
            //         {
            //             OnRedirectToLogin = ctx =>
            //             {
            //                 if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            //                 {
            //                     ctx.Response.StatusCode = 401;
            //                 }
            //
            //                 return Task.CompletedTask;
            //             },
            //             OnRedirectToAccessDenied = ctx =>
            //             {
            //                 if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            //                 {
            //                     ctx.Response.StatusCode = 403;
            //                 }
            //
            //                 return Task.CompletedTask;
            //             }
            //         };
            //     });

            services.AddIdentityServer()
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources());
            
            services.AddLocalApiAuthentication();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            
            services.AddLetsEncrypt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(DefaultCorsPolicy);

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DISABLE_WEB")))
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                
                    if (env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            }
        }
    }
}
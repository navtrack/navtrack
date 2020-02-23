using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Navtrack.Web.Services.LetsEncrypt
{
    public static class LetsEncryptExtension
    {
        public static IServiceCollection AddLetsEncrypt(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<KestrelServerOptions>, KestrelOptionsSetup>();
            services.AddSingleton<IHostedService, CertificateHostedService>()
                .AddSingleton<IStartupFilter, HttpChallengeStartupFilter>();

            return services;
        }
    }
}
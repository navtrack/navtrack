using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;

namespace Navtrack.Web.Services.LetsEncrypt
{
    internal class KestrelOptionsSetup : IConfigureOptions<KestrelServerOptions>
    {
        private readonly IServerCertificateSelector serverCertificateSelector;

        public KestrelOptionsSetup(IServerCertificateSelector serverCertificateSelector)
        {
            this.serverCertificateSelector = serverCertificateSelector;
        }

        public void Configure(KestrelServerOptions options)
        {
            options.ConfigureHttpsDefaults(configureOptions =>
            {
                configureOptions.ServerCertificateSelector = serverCertificateSelector.Select;
            });
        }
    }
}
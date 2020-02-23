using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(ICertificateService), ServiceLifetime.Singleton)]
    internal class CertificateService : ICertificateService
    {
        private readonly IEnumerable<ICertificateProvider> certificateProviders;
        private readonly IServerCertificateSelector serverCertificateSelector;

        public CertificateService(IEnumerable<ICertificateProvider> certificateProviders,
            IServerCertificateSelector serverCertificateSelector)
        {
            this.certificateProviders = certificateProviders;
            this.serverCertificateSelector = serverCertificateSelector;
        }

        public async Task Load(CancellationToken cancellationToken)
        {
            foreach (ICertificateProvider certificateProvider in certificateProviders)
            {
                IEnumerable<X509Certificate2> certificates =
                    await certificateProvider.GetCertificatesAsync(cancellationToken);

                foreach (X509Certificate2 certificate in certificates)
                {
                    serverCertificateSelector.Add(certificate);
                }
            }
        }
    }
}
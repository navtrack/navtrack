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
        private readonly ICertificateProvider certificateProvider;
        private readonly IServerCertificateSelector serverCertificateSelector;

        public CertificateService(ICertificateProvider certificateProvider, IServerCertificateSelector serverCertificateSelector)
        {
            this.certificateProvider = certificateProvider;
            this.serverCertificateSelector = serverCertificateSelector;
        }

        public async Task Load(CancellationToken cancellationToken)
        {
            X509Certificate2 certificate = await certificateProvider.GetCertificate();

            if (certificate != null)
            {
                serverCertificateSelector.Add(certificate);
            }
        }
    }
}
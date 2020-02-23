using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(ICertificateProvider), ServiceLifetime.Singleton)]
    internal class LetsEncryptCertificateProvider : ICertificateProvider
    {
        private readonly ICertificateDataService certificateDataService;
        private readonly ILetsEncryptClient letsEncryptClient;

        public LetsEncryptCertificateProvider(ICertificateDataService certificateDataService,
            ILetsEncryptClient letsEncryptClient)
        {
            this.certificateDataService = certificateDataService;
            this.letsEncryptClient = letsEncryptClient;
        }

        public async Task<IEnumerable<X509Certificate2>> GetCertificatesAsync(CancellationToken cancellationToken)
        {
            X509Certificate2 certificate = await certificateDataService.GetCertificate();
            
            if (certificate == null || certificate.NotAfter <= DateTime.Now)
            {
                certificate = await GetNewCertificate();
            }

            return certificate != null ? new[] {certificate} : Enumerable.Empty<X509Certificate2>();
        }

        private async Task<X509Certificate2> GetNewCertificate()
        {
            X509Certificate2 certificate = await letsEncryptClient.GetCertificate();
            
            if (certificate != null)
            {
                await certificateDataService.SaveAsync(certificate);
            }

            return certificate;
        }
    }
}
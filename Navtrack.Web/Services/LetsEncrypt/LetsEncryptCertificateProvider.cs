using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Common.Services;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(ICertificateProvider), ServiceLifetime.Singleton)]
    internal class LetsEncryptCertificateProvider : ICertificateProvider
    {
        private readonly ILetsEncryptClient letsEncryptClient;
        private readonly IDbConfiguration dbConfiguration;

        public LetsEncryptCertificateProvider(ILetsEncryptClient letsEncryptClient, IDbConfiguration dbConfiguration)
        {
            this.letsEncryptClient = letsEncryptClient;
            this.dbConfiguration = dbConfiguration;
        }

        public async Task<IEnumerable<X509Certificate2>> GetCertificatesAsync(CancellationToken cancellationToken)
        {
            X509Certificate2 certificate = await GetExistingCertificate();
            
            if (CertificateIsInvalid(certificate))
            {
                certificate = await CreateNewCertificate();
            }

            return certificate != null ? new[] {certificate} : Enumerable.Empty<X509Certificate2>();
        }

        private async Task<X509Certificate2> GetExistingCertificate()
        {
            string pfx = await dbConfiguration.Get<WebConfiguration>(x => x.Certificate);

            return !string.IsNullOrEmpty(pfx) ? new X509Certificate2(Convert.FromBase64String(pfx)) : null;
        }
        
        private async Task<X509Certificate2> CreateNewCertificate()
        {
            byte[] pfx = await letsEncryptClient.GetCertificate();

            if (pfx != null)
            {
                await dbConfiguration.Save<WebConfiguration>(x => x.Certificate, Convert.ToBase64String(pfx));

                return new X509Certificate2(pfx);
            }

            return null;
        }

        private static bool CertificateIsInvalid(X509Certificate2 certificate)
        {
            return certificate == null || certificate.NotAfter <= DateTime.Now;
        }

    }
}
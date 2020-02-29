using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Common.Services;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(ICertificateProvider), ServiceLifetime.Singleton)]
    public class CertificateProvider : ICertificateProvider
    { 
        private readonly IHostEnvironment hostEnvironment;
        private readonly IDbConfiguration dbConfiguration;
        private readonly ILetsEncryptClient letsEncryptClient;
        
        private const string AspNetHttpsOid = "1.3.6.1.4.1.311.84.1.1";

        public CertificateProvider(IHostEnvironment hostEnvironment, IDbConfiguration dbConfiguration,
            ILetsEncryptClient letsEncryptClient)
        {
            this.hostEnvironment = hostEnvironment;
            this.dbConfiguration = dbConfiguration;
            this.letsEncryptClient = letsEncryptClient;
        }

        public async Task<X509Certificate2> GetCertificate()
        {
            return hostEnvironment.IsDevelopment() ? GetDeveloperCertificate() : await GetDbCertificate();
        }
        
        private static X509Certificate2 GetDeveloperCertificate()
        {
            using X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);

            X509Certificate2 certificate = store.Certificates
                .Find(X509FindType.FindByExtension, AspNetHttpsOid, validOnly: false)
                .Cast<X509Certificate2>()
                .OrderByDescending(x => x.NotAfter).FirstOrDefault();

            return certificate;
        }

        private async Task<X509Certificate2> GetDbCertificate()
        {
            X509Certificate2 certificate = await GetExistingCertificate();
            
            if (CertificateIsInvalid(certificate))
            {
                certificate = await CreateNewCertificate();
            }

            return certificate;
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
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Common.Services;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(ICertificateDataService), ServiceLifetime.Singleton)]
    internal class CertificateDataService : ICertificateDataService
    {
        private readonly IDbConfiguration dbConfiguration;

        public CertificateDataService(IDbConfiguration dbConfiguration)
        {
            this.dbConfiguration = dbConfiguration;
        }

        public async Task<X509Certificate2> GetCertificate()
        {
            string result = await dbConfiguration.Get<WebConfiguration>(x => x.Certificate);

            if (!string.IsNullOrEmpty(result))
            {
                X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(result));

                return certificate;
            }

            return null;
        }

        public async Task SaveAsync(X509Certificate2 certificate)
        {
            await dbConfiguration.Save<WebConfiguration>(x => x.Certificate,
                Convert.ToBase64String(certificate.RawData));
        }
    }
}
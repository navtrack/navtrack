using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Navtrack.Library.DI;

namespace Navtrack.Web.Services.LetsEncrypt
{
    [Service(typeof(ICertificateProvider), ServiceLifetime.Singleton)]
    internal class DeveloperCertificateProvider : ICertificateProvider
    {
        private const string AspNetHttpsOid = "1.3.6.1.4.1.311.84.1.1";

        private readonly IHostEnvironment hostEnvironment;

        public DeveloperCertificateProvider(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public Task<IEnumerable<X509Certificate2>> GetCertificatesAsync(CancellationToken cancellationToken)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                return Task.FromResult(Enumerable.Empty<X509Certificate2>());
            }

            IEnumerable<X509Certificate2> certificates = GetDeveloperCertificates();

            return Task.FromResult(certificates);
        }

        private static IEnumerable<X509Certificate2> GetDeveloperCertificates()
        {
            using X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);

            List<X509Certificate2> certificates =
                store.Certificates.Find(X509FindType.FindByExtension, AspNetHttpsOid, validOnly: false)
                    .Cast<X509Certificate2>().ToList();

            return certificates;
        }
    }
}
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Web.Services.LetsEncrypt
{
    internal interface ICertificateProvider
    {
        Task<IEnumerable<X509Certificate2>> GetCertificatesAsync(CancellationToken cancellationToken);
    }
}
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Navtrack.Api.Services.LetsEncrypt
{
    public interface ICertificateProvider
    {
        Task<X509Certificate2> GetCertificate();
    }
}
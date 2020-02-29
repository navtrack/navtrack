using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Navtrack.Web.Services.LetsEncrypt
{
    public interface ICertificateProvider
    {
        Task<X509Certificate2> GetCertificate();
    }
}
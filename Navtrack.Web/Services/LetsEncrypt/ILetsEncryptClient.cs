using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Navtrack.Web.Services.LetsEncrypt
{
    internal interface ILetsEncryptClient
    {
        Task<X509Certificate2> GetCertificate();
    }
}
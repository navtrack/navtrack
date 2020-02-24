using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Navtrack.Web.Services.LetsEncrypt
{
    internal interface ILetsEncryptClient
    {
        Task<byte[]> GetCertificate();
    }
}
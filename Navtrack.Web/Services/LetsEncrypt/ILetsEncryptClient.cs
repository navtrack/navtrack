using System.Threading.Tasks;

namespace Navtrack.Web.Services.LetsEncrypt
{
    public interface ILetsEncryptClient
    {
        Task<byte[]> GetCertificate();
    }
}
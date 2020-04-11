using System.Threading.Tasks;

namespace Navtrack.Api.Services.LetsEncrypt
{
    public interface ILetsEncryptClient
    {
        Task<byte[]> GetCertificate();
    }
}
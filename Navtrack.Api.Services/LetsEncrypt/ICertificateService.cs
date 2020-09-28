using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Api.Services.LetsEncrypt
{
    internal interface ICertificateService
    {
        Task Load(CancellationToken cancellationToken);
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Web.Services.LetsEncrypt
{
    internal interface ICertificateService
    {
        Task Load(CancellationToken cancellationToken);
    }
}
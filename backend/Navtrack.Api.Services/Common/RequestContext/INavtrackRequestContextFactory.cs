using System.Threading.Tasks;

namespace Navtrack.Api.Services.Common.RequestContext;

public interface INavtrackRequestContextFactory
{
    Task CreateContext();
}
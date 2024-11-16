using System.Threading.Tasks;

namespace Navtrack.Api.Services.Common.Context;

public interface INavtrackContextFactory
{
    Task CreateContext();
}
using System.Threading.Tasks;

namespace Navtrack.Api.Services.IdentityServer;

public interface IExternalLoginHandler
{
    Task<string?> HandleToken(HandleTokenInput input);
}
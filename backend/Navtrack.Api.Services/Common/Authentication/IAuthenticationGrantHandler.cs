using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace Navtrack.Api.Services.Common.Authentication;

public interface IAuthenticationGrantHandler
{
    string GrantType { get; }
    Task<AuthenticationGrantResult> Authenticate(OpenIddictRequest request);
}
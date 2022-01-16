namespace Navtrack.Api.Services.IdentityServer.Model;

public class AppleAuthenticationSettings : ExternalAuthenticationSettings
{
    public string RedirectUri { get; set; }
}
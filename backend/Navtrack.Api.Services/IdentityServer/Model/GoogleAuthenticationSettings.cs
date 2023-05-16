namespace Navtrack.Api.Services.IdentityServer.Model;

public class GoogleAuthenticationSettings : ExternalAuthenticationSettings
{
    public string ClientSecret { get; set; }
}
namespace Navtrack.Api.Services.IdentityServer.Model;

public class ExternalAuthenticationSettings
{
    public string Issuer { get; set; }
    public string ClientId { get; set; }
    public string MetadataAddress { get; set; }
}
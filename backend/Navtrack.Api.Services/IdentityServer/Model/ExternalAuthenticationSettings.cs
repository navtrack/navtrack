namespace Navtrack.Api.Services.IdentityServer.Model;

public class ExternalAuthenticationSettings
{
    public string[] ValidIssuers { get; set; }
    public string[] ValidAudiences { get; set; }
    public string MetadataAddress { get; set; }
}
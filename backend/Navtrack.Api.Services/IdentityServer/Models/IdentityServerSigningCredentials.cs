namespace Navtrack.Api.Services.IdentityServer.Models;

public class IdentityServerSigningCredentials
{
    public string Key { get; set; }
    public KeyParameters KeyParameters { get; set; }
}
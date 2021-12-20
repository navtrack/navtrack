namespace Navtrack.Api.Services.IdentityServer.Model;

public class KeyParameters
{
    public byte[] D { get; set; }

    public byte[] DP { get; set; }

    public byte[] DQ { get; set; }

    public byte[] Exponent { get; set; }

    public byte[] InverseQ { get; set; }

    public byte[] Modulus { get; set; }

    public byte[] P { get; set; }

    public byte[] Q { get; set; }
}
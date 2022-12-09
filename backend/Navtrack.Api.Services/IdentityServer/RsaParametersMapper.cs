using System.Security.Cryptography;
using Navtrack.Api.Services.IdentityServer.Model;

namespace Navtrack.Api.Services.IdentityServer;

public static class RsaParametersMapper
{
    public static KeyParameters Map(RSAParameters source)
    {
        KeyParameters keyParameters = new()
        {
            D = source.D,
            DP = source.DP,
            DQ = source.DQ,
            Exponent = source.Exponent,
            InverseQ = source.InverseQ,
            Modulus = source.Modulus,
            P = source.P,
            Q = source.Q
        };

        return keyParameters;
    }

    public static RSAParameters Map(KeyParameters source)
    {
        RSAParameters parameters = new()
        {
            D = source.D,
            DP = source.DP,
            DQ = source.DQ,
            Exponent = source.Exponent,
            InverseQ = source.InverseQ,
            Modulus = source.Modulus,
            P = source.P,
            Q = source.Q
        };

        return parameters;
    }
}
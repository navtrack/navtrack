using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

internal static class AccessTokenElementMapper
{
    public static AccessTokenElement Map(Token source)
    {
        return new AccessTokenElement
        {
            Claims = source.Claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value)),
            AllowedSigningAlgorithms = source.AllowedSigningAlgorithms,
            Confirmation = source.Confirmation,
            Audiences = source.Audiences,
            Issuer = source.Issuer,
            CreationTime = source.CreationTime,
            Lifetime = source.Lifetime,
            Type = source.Type,
            ClientId = source.ClientId,
            AccessTokenType = source.AccessTokenType.ToString(),
            Description = source.Description,
            Version = source.Version
        };
    }
}
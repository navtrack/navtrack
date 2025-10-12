using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using Navtrack.Database.Model.Authentication;

namespace Navtrack.Api.Services.Common.IdentityServer.Mappers;

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
            SubjectId = source.SubjectId,
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
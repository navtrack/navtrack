using System;
using System.Linq;
using System.Security.Claims;
using IdentityServer4.Models;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

internal static class TokenMapper
{
    public static Token Map(AccessTokenElement source)
    {
        return new Token
        {
            Claims = source.Claims.Select(x => new Claim(x.Key, x.Value)).ToList(),
            AllowedSigningAlgorithms = source.AllowedSigningAlgorithms.ToList(),
            Confirmation = source.Confirmation,
            Audiences = source.Audiences.ToList(),
            Issuer = source.Issuer,
            CreationTime = source.CreationTime,
            Lifetime = source.Lifetime,
            Type = source.Type,
            ClientId = source.ClientId,
            AccessTokenType = Enum.Parse<AccessTokenType>(source.AccessTokenType),
            Description = source.Description,
            Version = source.Version
        };
    }
}
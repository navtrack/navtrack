using System;
using System.Linq;
using IdentityServer4.Models;
using Navtrack.DataAccess.Model.Users.RefreshTokens;
using Navtrack.Shared.Library.Utils;

namespace Navtrack.Api.Services.IdentityServer.Mappers;

public static class RefreshTokenDocumentMapper
{
    public static RefreshTokenDocument Map(RefreshToken source)
    {
        string jwtId = source.AccessToken.Claims.First(x => x.Type == "jti").Value;
        
        return new RefreshTokenDocument
        {
            Hash = HashUtil.GenerateSha256Hash(Guid.NewGuid().ToString()),
            CreationTime = source.CreationTime,
            ExpiryTime = source.CreationTime.AddSeconds(source.Lifetime),
            Lifetime = source.Lifetime,
            ConsumedTime = source.ConsumedTime,
            AccessToken = AccessTokenElementMapper.Map(source.AccessToken),
            Version = source.Version,
            JwtId = jwtId
        };
    }
}
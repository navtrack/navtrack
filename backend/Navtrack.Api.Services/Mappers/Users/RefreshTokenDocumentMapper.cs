using System;
using System.Linq;
using IdentityServer4.Models;
using Navtrack.Core.Services.Util;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers.Users;

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
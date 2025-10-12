using System;
using System.Linq;
using IdentityServer4.Models;
using Navtrack.Database.Model.Authentication;
using Navtrack.Shared.Library.Utils;
using Newtonsoft.Json;

namespace Navtrack.Api.Services.Common.IdentityServer.Mappers;

public static class RefreshTokenEntityMapper
{
    public static AuthRefreshTokenEntity Map(RefreshToken source)
    {
        string jwtId = source.AccessToken.Claims.First(x => x.Type == "jti").Value;
        
        return new AuthRefreshTokenEntity
        {
            Hash = HashUtil.GenerateSha256Hash(Guid.NewGuid().ToString()),
            CreationTime = source.CreationTime,
            ExpiryTime = source.CreationTime.AddSeconds(source.Lifetime),
            Lifetime = source.Lifetime,
            ConsumedTime = source.ConsumedTime,
            AccessToken = JsonConvert.SerializeObject(AccessTokenElementMapper.Map(source.AccessToken)),
            Version = source.Version,
            JwtId = jwtId,
            SubjectId = source.AccessToken.SubjectId,
            ClientId = source.AccessToken.ClientId
        };
    }
}
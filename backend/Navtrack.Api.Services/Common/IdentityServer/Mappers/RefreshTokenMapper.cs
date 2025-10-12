using IdentityServer4.Models;
using Navtrack.Database.Model.Authentication;
using Newtonsoft.Json;

namespace Navtrack.Api.Services.Common.IdentityServer.Mappers;

internal static class RefreshTokenMapper
{
    public static RefreshToken Map(AuthRefreshTokenEntity source)
    {
        return new RefreshToken
        {
            CreationTime = source.CreationTime,
            Lifetime = source.Lifetime,
            ConsumedTime = source.ConsumedTime,
            AccessToken = AccessTokenMapper.Map(JsonConvert.DeserializeObject<AccessTokenElement>(source.AccessToken)),
            Version = source.Version
        };
    }
}
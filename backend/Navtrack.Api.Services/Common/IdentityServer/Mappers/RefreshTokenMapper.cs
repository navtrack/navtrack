using IdentityServer4.Models;
using Navtrack.DataAccess.Model.Users.RefreshTokens;

namespace Navtrack.Api.Services.Common.IdentityServer.Mappers;

internal static class RefreshTokenMapper
{
    public static RefreshToken Map(RefreshTokenDocument source)
    {
        return new RefreshToken
        {
            CreationTime = source.CreationTime,
            Lifetime = source.Lifetime,
            ConsumedTime = source.ConsumedTime,
            AccessToken = AccessTokenMapper.Map(source.AccessToken),
            Version = source.Version
        };
    }
}
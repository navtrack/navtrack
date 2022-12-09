using IdentityServer4.Models;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

internal static class RefreshTokenMapper
{
    public static RefreshToken Map(RefreshTokenDocument source)
    {
        return new RefreshToken
        {
            CreationTime = source.CreationTime,
            Lifetime = source.Lifetime,
            ConsumedTime = source.ConsumedTime,
            AccessToken = TokenMapper.Map(source.AccessToken),
            Version = source.Version,
        };
    }
}
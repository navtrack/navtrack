using IdentityServer4.Models;
using MongoDB.Bson;
using Navtrack.Api.Services.IdentityServer;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Mappers;

public static class RefreshTokenDocumentMapper
{
    public static RefreshTokenDocument Map(RefreshToken source)
    {
        return new RefreshTokenDocument
        {
            Id = ObjectId.Parse(source.Subject.GetId()),
            CreationTime = source.CreationTime,
            Lifetime = source.Lifetime,
            ConsumedTime = source.ConsumedTime,
            AccessToken = AccessTokenElementMapper.Map(source.AccessToken),
            Version = source.Version
        };
    }
}
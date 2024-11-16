using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class AssetTeamElementMapper
{
    public static AssetTeamElement Map(ObjectId teamId, ObjectId currentUserId)
    {
        return new AssetTeamElement
        {
            TeamId = teamId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = currentUserId
        };
    }
}
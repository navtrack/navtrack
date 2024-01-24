using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Model.New;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Assets;

public interface IAssetRepository : IGenericRepository<AssetDocument>
{
    Task<AssetDocument?> Get(string serialNumber, int protocolPort);
    Task<List<AssetDocument>> GetAssetsByIds(List<ObjectId> ids);
    Task<bool> NameIsUsed(string name, ObjectId ownerUserId, string? assetId = null);
    Task UpdateName(string assetId, string name);
    Task AddUserToAsset(AssetDocument assetDocument, UserDocument? userDocument, AssetRoleType modelRole);
    Task RemoveUserFromAsset(string assetId, string userId);
    Task UpdateLocation(ObjectId assetId, LocationDocument location);
    Task SetActiveDevice(ObjectId assetId, ObjectId deviceId, string serialNumber, string deviceTypeId,
        int protocolPort);
    Task SetPosition(ObjectId assetId, PositionElement position);
    Task<bool> IsNewerPositionDate(ObjectId assetId, DateTime date);
}
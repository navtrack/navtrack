using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Assets;

public interface IAssetDataService
{
    Task<AssetDocument> GetById(string id);
    Task<List<AssetDocument>> GetAssetsByIds(List<ObjectId> ids);
    Task<bool> NameIsUsed(string name, ObjectId ownerUserId, string? assetId = null);
    Task UpdateName(string assetId, string name);
    Task Delete(string assetId);
    Task AddUserToAsset(AssetDocument assetDocument, UserDocument userDocument, AssetRoleType modelRole);
    Task RemoveUserFromAsset(string assetId, string userId);
    Task UpdateLocation(ObjectId assetId, LocationDocument location);
    Task SetActiveDevice(ObjectId assetId, ObjectId deviceId, string serialNumber, string deviceTypeId,
        int protocolPort);
}
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Assets;

public interface IAssetRepository : IGenericRepository<AssetDocument>
{
    Task<AssetDocument> Get(string serialNumber, int protocolPort);
    Task<List<AssetDocument>> GetAssetsByIds(List<ObjectId> ids);
    Task<bool> NameIsUsed(string name, ObjectId ownerUserId, string? assetId = null);
    Task UpdateName(string assetId, string name);
    Task AddUserToAsset(AssetUserRoleElement userRole, UserAssetRoleElement assetRole);
    Task RemoveUserFromAsset(string assetId, string userId);
    Task SetLastPositionMessage(ObjectId assetId, DeviceMessageDocument deviceMessage);
    Task SetActiveDevice(ObjectId assetDocumentId, AssetDeviceElement assetDocumentDevice);
}
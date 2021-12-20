using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Assets;

public interface IAssetDataService
{
    Task<AssetDocument> GetById(string id);
    Task<List<AssetDocument>> GetAssetsByIds(List<ObjectId> ids);
    Task<bool> NameIsUsed(string name, ObjectId ownerUserId, string assetId = null);
    Task UpdateName(string assetId, string name);
    Task Delete(string assetId);
    Task AddUserToAsset(AssetDocument assetDocument, UserDocument userDocument, AssetRoleType modelRole);
    Task RemoveUserFromAsset(string assetId, string userId);


    Task<bool> UserHasRoleForAsset(string userId, AssetRoleType assetRoleType, string assetId);
    Task<bool> UserHasRolesForAsset(string userId, AssetRoleType[] assetRoleTypes, string assetId);
    Task<bool> UserHasRoleForDevice(string userId, AssetRoleType assetRoleType, string deviceId);
    Task<bool> HasActiveDeviceId(string assetId, string deviceId);
}
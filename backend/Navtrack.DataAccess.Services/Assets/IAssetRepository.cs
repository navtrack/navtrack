using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Assets;

public interface IAssetRepository : IGenericRepository<AssetDocument>
{
    Task<AssetDocument> Get(string serialNumber, int protocolPort);
    Task<bool> NameIsUsed(ObjectId organizationId, string name, ObjectId? assetId = null);
    Task UpdateName(string assetId, string name);
    Task UpdateMessages(ObjectId assetId, DeviceMessageDocument lastMessage, DeviceMessageDocument? positionMessage);
    Task SetActiveDevice(ObjectId assetId, AssetDeviceElement assetDevice);
    Task<List<AssetDocument>> GetByTeamId(ObjectId teamId);
    Task<List<AssetDocument>> GetByOrganizationId(ObjectId organizationId);
    Task<List<AssetDocument>> GetAssetsByAssetAndTeamIds(List<ObjectId> assetIds, List<ObjectId> teamIds);
    
    Task AddAssetToTeam(ObjectId assetId, AssetTeamElement element);
    Task RemoveAssetFromTeam(ObjectId teamId, ObjectId assetId);
    Task RemoveTeamFromAssets(ObjectId teamId);
}
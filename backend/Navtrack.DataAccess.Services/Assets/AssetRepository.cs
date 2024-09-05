using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Assets;

[Service(typeof(IAssetRepository))]
public class AssetRepository(IRepository repository) : GenericRepository<AssetDocument>(repository), IAssetRepository
{
    public Task<AssetDocument> Get(string serialNumber, int protocolPort)
    {
        Task<AssetDocument>? asset = repository.GetQueryable<AssetDocument>()
            .FirstOrDefaultAsync(x =>
                x.Device != null &&
                x.Device.SerialNumber == serialNumber &&
                x.Device.ProtocolPort == protocolPort);

        return asset;
    }

    public Task<List<AssetDocument>> GetAssetsByIds(List<ObjectId> ids)
    {
        return repository.GetQueryable<AssetDocument>()
            .Where(x => ids.Contains(x.Id))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public Task<bool> NameIsUsed(string name, ObjectId ownerUserId, string? assetId = null)
    {
        name = name.ToLower();

        Expression<Func<AssetDocument, bool>> filter = assetId == null
            ? x =>
                x.UserRoles.Any(y => y.Role == AssetRoleType.Owner && y.UserId == ownerUserId) &&
                x.Name.ToLower().Equals(name.ToLower())
            : x =>
                x.UserRoles.Any(y => y.Role == AssetRoleType.Owner && y.UserId == ownerUserId) &&
                x.Name.ToLower().Equals(name.ToLower()) &&
                x.Id != ObjectId.Parse(assetId);

        return repository.GetQueryable<AssetDocument>()
            .AnyAsync(filter);
    }

    public async Task UpdateName(string assetId, string name)
    {
        UpdateDefinition<AssetDocument> update = Builders<AssetDocument>.Update.Set(x => x.Name, name);

        await repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == ObjectId.Parse(assetId), update);
    }

    public async Task AddUserToAsset(AssetUserRoleElement userRole, UserAssetRoleElement assetRole)
    {
        await repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetRole.AssetId,
                Builders<AssetDocument>.Update.Push(x => x.UserRoles, userRole));

        await repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userRole.UserId,
                Builders<UserDocument>.Update.Push(x => x.AssetRoles, assetRole));
    }

    public async Task RemoveUserFromAsset(string assetId, string userId)
    {
        ObjectId assetObjectId = ObjectId.Parse(assetId);
        ObjectId userObjectId = ObjectId.Parse(userId);

        await repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetObjectId,
                Builders<AssetDocument>.Update.PullFilter(x => x.UserRoles, x => x.UserId == userObjectId));

        await repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userObjectId,
                Builders<UserDocument>.Update.PullFilter(x => x.AssetRoles, x => x.AssetId == assetObjectId));
    }

    public Task SetActiveDevice(ObjectId assetId, ObjectId deviceId, string serialNumber, string deviceTypeId,
        int protocolPort)
    {
        return repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetId,
                Builders<AssetDocument>.Update.Set(x => x.Device!.Id, deviceId)
                    .Set(x => x.Device!.SerialNumber, serialNumber)
                    .Set(x => x.Device!.ProtocolPort, protocolPort)
                    .Set(x => x.Device!.DeviceTypeId, deviceTypeId));
    }

    public Task UpdateMessages(ObjectId assetId, DeviceMessageDocument lastMessage,
        DeviceMessageDocument? positionMessage)
    {
        List<UpdateDefinition<AssetDocument>> updateDefinitions =
            [Builders<AssetDocument>.Update.Set(x => x.LastMessage, lastMessage)];

        if (positionMessage != null)
        {
            updateDefinitions.Add(Builders<AssetDocument>.Update.Set(x => x.LastPositionMessage, positionMessage));
        }

        return repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetId, Builders<AssetDocument>.Update.Combine(updateDefinitions));
    }

    public Task SetActiveDevice(ObjectId assetDocumentId, AssetDeviceElement assetDocumentDevice)
    {
        return repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetDocumentId,
                Builders<AssetDocument>.Update.Set(x => x.Device, assetDocumentDevice));
    }
}
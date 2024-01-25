using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Assets;

[Service(typeof(IAssetRepository))]
public class AssetRepository(IRepository repository) : GenericRepository<AssetDocument>(repository), IAssetRepository
{
    public Task<AssetDocument?> Get(string serialNumber, int protocolPort)
    {
        return repository.GetQueryable<AssetDocument>()
            .FirstOrDefaultAsync(x => x.Device.SerialNumber == serialNumber && x.Device.ProtocolPort == protocolPort);
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
                x.Name.ToLower() == name
            : x =>
                x.UserRoles.Any(y => y.Role == AssetRoleType.Owner && y.UserId == ownerUserId) &&
                x.Name.ToLower() == name &&
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

    public async Task AddUserToAsset(AssetDocument assetDocument, UserDocument? userDocument,
        AssetRoleType modelRole)
    {
        UserAssetRoleElement userAssetRoleElement = new()
        {
            Id = ObjectId.GenerateNewId(),
            AssetId = assetDocument.Id,
            Role = modelRole
        };

        AssetUserRoleElement assetUserRoleElement = new()
        {
            Id = userAssetRoleElement.Id,
            UserId = userDocument.Id,
            Role = modelRole
        };

        await repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetDocument.Id,
                Builders<AssetDocument>.Update.Push(x => x.UserRoles, assetUserRoleElement));

        await repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userDocument.Id,
                Builders<UserDocument>.Update.Push(x => x.AssetRoles, userAssetRoleElement));
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
                Builders<AssetDocument>.Update.Set(x => x.Device.Id, deviceId)
                    .Set(x => x.Device.SerialNumber, serialNumber)
                    .Set(x => x.Device.ProtocolPort, protocolPort)
                    .Set(x => x.Device.DeviceTypeId, deviceTypeId));
    }

    public Task SetPosition(ObjectId assetId, PositionElement position)
    {
        return repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetId,
                Builders<AssetDocument>.Update.Set(x => x.Position, position));
    }

    public Task<bool> IsNewerPositionDate(ObjectId assetId, DateTime date)
    {
        return repository.GetQueryable<AssetDocument>()
            .AnyAsync(x => x.Id == assetId && (x.Position == null || x.Position.Date < date));
    }
}
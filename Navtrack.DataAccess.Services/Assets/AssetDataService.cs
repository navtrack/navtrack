using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Assets;

[Service(typeof(IAssetDataService))]
public class AssetDataService : IAssetDataService
{
    private readonly IRepository repository;

    public AssetDataService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<AssetDocument> GetById(string id)
    {
        AssetDocument asset = await repository.GetEntities<AssetDocument>()
            .FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(id));

        return asset;
    }

    public Task<List<AssetDocument>> GetAssetsByIds(List<ObjectId> ids)
    {
        return repository.GetEntities<AssetDocument>()
            .Where(x => ids.Contains(x.Id))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public Task<bool> NameIsUsed(string name, ObjectId ownerUserId, string assetId = null)
    {
        Expression<Func<AssetDocument, bool>> filter = assetId == null
            ? x =>
                x.UserRoles.Any(y => y.Role == AssetRoleType.Owner && y.UserId == ownerUserId) &&
                x.Name.ToLower() == name
            : x =>
                x.UserRoles.Any(y => y.Role == AssetRoleType.Owner && y.UserId == ownerUserId) &&
                x.Name.ToLower() == name &&
                x.Id != ObjectId.Parse(assetId);

        return repository.GetEntities<AssetDocument>()
            .AnyAsync(filter);
    }

    public async Task UpdateName(string assetId, string name)
    {
        UpdateDefinition<AssetDocument> update = Builders<AssetDocument>.Update.Set(x => x.Name, name);

        await repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == ObjectId.Parse(assetId), update);
    }

    public Task Delete(string assetId)
    {
        return repository.GetCollection<AssetDocument>().DeleteOneAsync(x => x.Id == ObjectId.Parse(assetId));
    }

    public async Task AddUserToAsset(AssetDocument assetDocument, UserDocument userDocument,
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
        
        
        
        
        
        
    public Task<bool> UserHasRoleForAsset(string userId, AssetRoleType assetRoleType, string assetId)
    {
        int roleId = (int)assetRoleType;

        return repository.GetEntities<UserDocument>().AnyAsync(x =>
            x.Id == ObjectId.Parse(userId) &&
            x.AssetRoles.Any(y => y.AssetId == ObjectId.Parse(assetId) && y.Role == assetRoleType));
    }

    public Task<bool> UserHasRolesForAsset(string userId, AssetRoleType[] assetRoleTypes, string assetId)
    {
        int[] roleIds = assetRoleTypes.Select(x => (int)x).ToArray();

        return repository.GetEntities<UserDocument>().AnyAsync(x =>
            x.Id == ObjectId.Parse(userId) &&
            x.AssetRoles.Any(y => y.AssetId == ObjectId.Parse(assetId) && assetRoleTypes.Contains(y.Role)));
    }

    public async Task<bool> UserHasRoleForDevice(string userId, AssetRoleType assetRoleType, string deviceId)
    {
        DeviceDocument device = await repository.GetEntities<DeviceDocument>()
            .FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(deviceId));

        if (device != null)
        {
            bool result = await repository.GetEntities<UserDocument>().AnyAsync(x =>
                x.Id == ObjectId.Parse(userId) &&
                x.AssetRoles.Any(y => y.Role == assetRoleType && y.AssetId == device.AssetId));

            return result;
        }

        return false;
    }

    public Task<bool> HasActiveDeviceId(string assetId, string deviceId)
    {
        return repository.GetEntities<AssetDocument>()
            .AnyAsync(x => x.Id == ObjectId.Parse(assetId) && x.Device.Id == ObjectId.Parse(deviceId));
    }
}
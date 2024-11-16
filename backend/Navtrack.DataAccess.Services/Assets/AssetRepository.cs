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

    public Task<bool> NameIsUsed(ObjectId organizationId, string name, ObjectId? assetId = null)
    {
        Expression<Func<AssetDocument, bool>> filter = assetId == null
            ? x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower())
            : x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower()) &&
                x.Id != assetId;

        return repository.GetQueryable<AssetDocument>()
            .AnyAsync(filter);
    }

    public async Task UpdateName(string assetId, string name)
    {
        UpdateDefinition<AssetDocument> update = Builders<AssetDocument>.Update.Set(x => x.Name, name);

        await repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == ObjectId.Parse(assetId), update);
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

    public Task SetActiveDevice(ObjectId assetId, AssetDeviceElement assetDevice)
    {
        return repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetId,
                Builders<AssetDocument>.Update.Set(x => x.Device, assetDevice));
    }

    public Task<List<AssetDocument>> GetByTeamId(ObjectId teamId)
    {
        return repository.GetQueryable<AssetDocument>()
            .Where(x => x.Teams!.Any(y => y.TeamId == teamId))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public Task<List<AssetDocument>> GetByOrganizationId(ObjectId organizationId)
    {
        return repository.GetQueryable<AssetDocument>()
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public Task<List<AssetDocument>> GetAssetsByAssetAndTeamIds(List<ObjectId> assetIds, List<ObjectId> teamIds)
    {
        return repository.GetQueryable<AssetDocument>()
            .Where(x => assetIds.Contains(x.Id) || x.Teams!.Any(y => teamIds.Contains(y.TeamId)))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<AssetDocument>> GetByUserId(ObjectId userId, ObjectId organizationId)
    {
        UserDocument user = await repository.GetQueryable<UserDocument>().FirstOrDefaultAsync(x => x.Id == userId);

        List<ObjectId> assetIds =
            user?.Assets?.Where(x => x.OrganizationId == organizationId).Select(x => x.AssetId).ToList() ?? [];

        List<ObjectId> teamIds =
            user?.Teams?.Where(x => x.OrganizationId == organizationId).Select(x => x.TeamId).ToList() ?? [];

        List<AssetDocument> assets = await repository.GetQueryable<AssetDocument>()
            .Where(x => x.OrganizationId == organizationId &&
                        (assetIds.Contains(x.Id) || x.Teams!.Any(y => teamIds.Contains(y.TeamId))))
            .OrderBy(x => x.Name)
            .ToListAsync();

        return assets;
    }

    public Task AddAssetToTeam(ObjectId assetId, AssetTeamElement element)
    {
        return repository.GetCollection<AssetDocument>().UpdateOneAsync(x => x.Id == assetId,
            Builders<AssetDocument>.Update.Push(x => x.Teams, element));
    }

    public Task RemoveAssetFromTeam(ObjectId teamId, ObjectId assetId)
    {
        return repository.GetCollection<AssetDocument>()
            .UpdateOneAsync(x => x.Id == assetId,
                Builders<AssetDocument>.Update.PullFilter(x => x.Teams, x => x.TeamId == teamId));
    }

    public Task RemoveTeamFromAssets(ObjectId teamId)
    {
        return repository.GetCollection<AssetDocument>()
            .UpdateManyAsync(x => x.Teams!.Any(y => y.TeamId == teamId),
                Builders<AssetDocument>.Update.PullFilter(x => x.Teams, y => y.TeamId == teamId));
    }
}
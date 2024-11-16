using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Organizations;

[Service(typeof(IOrganizationRepository))]
public class OrganizationRepository(IRepository repository)
    : GenericRepository<OrganizationDocument>(repository), IOrganizationRepository
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

    public override Task<List<OrganizationDocument>> GetByIds(IEnumerable<ObjectId> ids)
    {
        return repository.GetQueryable<OrganizationDocument>()
            .Where(x => ids.Contains(x.Id))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task UpdateName(string assetId, string name)
    {
        UpdateDefinition<OrganizationDocument> update = Builders<OrganizationDocument>.Update.Set(x => x.Name, name);

        await repository.GetCollection<OrganizationDocument>()
            .UpdateOneAsync(x => x.Id == ObjectId.Parse(assetId), update);
    }

    public Task UpdateAssetsCount(ObjectId organizationId)
    {
        int assets = repository.GetQueryable<AssetDocument>()
            .Count(x => x.OrganizationId == organizationId);

        return repository.GetCollection<OrganizationDocument>()
            .UpdateOneAsync(x => x.Id == organizationId,
                Builders<OrganizationDocument>.Update.Set(x => x.AssetsCount, assets));
    }

    public Task UpdateUsersCount(ObjectId organizationId)
    {
        int users = repository.GetQueryable<UserDocument>()
            .Count(x => x.Organizations.Any(y => y.OrganizationId == organizationId));

        return repository.GetCollection<OrganizationDocument>()
            .UpdateOneAsync(x => x.Id == organizationId,
                Builders<OrganizationDocument>.Update.Set(x => x.UsersCount, users));
    }

    public Task UpdateTeamsCount(ObjectId organizationId)
    {
        int teams = repository.GetQueryable<TeamDocument>()
            .Count(x => x.OrganizationId == organizationId);

        return repository.GetCollection<OrganizationDocument>()
            .UpdateOneAsync(x => x.Id == organizationId,
                Builders<OrganizationDocument>.Update.Set(x => x.TeamsCount, teams));
    }
}
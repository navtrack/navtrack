using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Teams;

[Service(typeof(ITeamRepository))]
public class TeamRepository(IRepository repository) : GenericRepository<TeamDocument>(repository), ITeamRepository
{
    public Task<bool> NameIsUsed(string name, ObjectId organizationId, ObjectId? teamId = null)
    {
        Expression<Func<TeamDocument, bool>> filter = teamId == null
            ? x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower())
            : x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower()) &&
                x.Id != teamId;

        return repository.GetQueryable<TeamDocument>()
            .AnyAsync(filter);
    }

    public async Task UpdateName(ObjectId teamId, string name)
    {
        UpdateDefinition<TeamDocument> update = Builders<TeamDocument>.Update.Set(x => x.Name, name);

        await repository.GetCollection<TeamDocument>()
            .UpdateOneAsync(x => x.Id == teamId, update);
    }

    public Task<List<TeamDocument>> GetByOrganizationId(ObjectId organizationId)
    {
        return repository.GetQueryable<TeamDocument>()
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task UpdateUsersCount(ObjectId teamId)
    {
        int usersCount = await repository.GetCollection<UserDocument>()
            .AsQueryable()
            .CountAsync(x => x.Teams!.Any(y => y.TeamId == teamId));

        await repository.GetCollection<TeamDocument>()
            .UpdateOneAsync(x => x.Id == teamId,
                Builders<TeamDocument>.Update.Set(x => x.UsersCount, usersCount));
    }

    public async Task UpdateAssetsCount(ObjectId teamId)
    {
        int assetsCount = await repository.GetCollection<AssetDocument>()
            .AsQueryable()
            .CountAsync(x => x.Teams!.Any(y => y.TeamId == teamId));

        await repository.GetCollection<TeamDocument>()
            .UpdateOneAsync(x => x.Id == teamId,
                Builders<TeamDocument>.Update.Set(x => x.AssetsCount, assetsCount));
    }
}
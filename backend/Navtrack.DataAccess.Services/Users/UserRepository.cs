using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Users;

[Service(typeof(IUserRepository))]
public class UserRepository(IRepository repository) : GenericRepository<UserDocument>(repository), IUserRepository
{
    public async Task<UserDocument?> GetByEmail(string email)
    {
        email = LowerAndTrim(email);

        UserDocument? user = await repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        return user;
    }

    public Task<bool> EmailIsUsed(string email)
    {
        email = LowerAndTrim(email);

        return repository.GetQueryable<UserDocument>().AnyAsync(x => x.Email == email);
    }

    public async Task Update(ObjectId id, UpdateUser updateUser)
    {
        List<UpdateDefinition<UserDocument>> updateDefinitions = [];

        if (!string.IsNullOrEmpty(updateUser.Email))
        {
            updateDefinitions.Add(Builders<UserDocument>.Update.Set(x => x.Email, updateUser.Email.ToLower()));
        }

        if (updateUser.UnitsType.HasValue)
        {
            updateDefinitions.Add(Builders<UserDocument>.Update.Set(x => x.UnitsType, updateUser.UnitsType.Value));
        }

        if (updateUser.Password != null)
        {
            updateDefinitions.Add(Builders<UserDocument>.Update.Set(x => x.Password, updateUser.Password));
        }

        if (updateDefinitions.Count != 0)
        {
            await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == id,
                Builders<UserDocument>.Update.Combine(updateDefinitions));
        }
    }

    public Task<List<UserDocument>> GetByOrganizationId(ObjectId organizationId)
    {
        return repository.GetQueryable<UserDocument>()
            .Where(x => x.Organizations!.Any(y => y.OrganizationId == organizationId))
            .OrderBy(x => x.Email)
            .ToListAsync();
    }

    public Task<int> GetOrganizationOwnersCount(ObjectId organizationId)
    {
        return repository.GetQueryable<UserDocument>()
            .CountAsync(x => x.Organizations!.Any(y => y.OrganizationId == organizationId &&
                                                       y.UserRole == OrganizationUserRole.Owner));
    }

    public Task<List<UserDocument>> GetByTeamId(string teamId)
    {
        return repository.GetQueryable<UserDocument>()
            .Where(x => x.Teams!.Any(y => y.TeamId == ObjectId.Parse(teamId)))
            .OrderBy(x => x.Email)
            .ToListAsync();
    }

    public Task<List<UserDocument>> GetByAssetId(ObjectId assetId)
    {
        return repository.GetQueryable<UserDocument>()
            .Where(x => x.Assets!.Any(y => y.AssetId == assetId))
            .OrderBy(x => x.Email)
            .ToListAsync();
    }

    public Task RemoveAssetFromUsers(ObjectId assetId)
    {
        return repository.GetCollection<UserDocument>().UpdateManyAsync(
            x => x.Assets!.Any(y => y.AssetId == assetId),
            Builders<UserDocument>.Update.PullFilter(x => x.Assets, x => x.AssetId == assetId));
    }

    private static string LowerAndTrim(string email)
    {
        email = email.ToLower().Trim();

        return email;
    }

    public Task AddAssetToUser(ObjectId userId, UserAssetElement asset)
    {
        return repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userId,
                Builders<UserDocument>.Update.Push(x => x.Assets, asset));
    }

    public Task RemoveAssetFromUser(ObjectId assetId, ObjectId userId)
    {
        return repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userId,
                Builders<UserDocument>.Update.PullFilter(x => x.Assets, x => x.AssetId == assetId));
    }

    public async Task AddUserToTeam(ObjectId userId, UserTeamElement element)
    {
        await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == userId,
            Builders<UserDocument>.Update.Push(x => x.Teams, element));

        await repository.GetCollection<TeamDocument>()
            .UpdateOneAsync(x => x.Id == element.TeamId,
                Builders<TeamDocument>.Update.Inc(x => x.UsersCount, 1));
    }

    public Task UpdateTeamUser(ObjectId teamId, ObjectId userId, TeamUserRole userRole)
    {
        return repository.GetCollection<UserDocument>().UpdateOneAsync(
            x => x.Id == userId && x.Teams!.Any(y =>
                y.TeamId == teamId),
            Builders<UserDocument>.Update.Set(x => x.Teams.FirstMatchingElement().UserRole, userRole));
    }

    public Task RemoveTeamFromUser(ObjectId userId, ObjectId teamId)
    {
        return repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userId,
                Builders<UserDocument>.Update.PullFilter(x => x.Teams, x => x.TeamId == teamId));
    }

    public Task RemoveTeamFromUsers(ObjectId teamId)
    {
        return repository.GetCollection<UserDocument>()
            .UpdateManyAsync(x => x.Teams!.Any(y => y.TeamId == teamId),
                Builders<UserDocument>.Update.PullFilter(x => x.Teams, y => y.TeamId == teamId));
    }
    
    public Task UpdateOrganizationUser(ObjectId userId, ObjectId organizationId, OrganizationUserRole userRole)
    {
        return repository.GetCollection<UserDocument>().UpdateOneAsync(
            x => x.Id == userId && x.Organizations!.Any(y =>
                y.OrganizationId == organizationId),
            Builders<UserDocument>.Update.Set(x => x.Organizations.FirstMatchingElement().UserRole, userRole));
    }

    public Task AddUserToOrganization(ObjectId userId, UserOrganizationElement element)
    {
        return repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == userId,
            Builders<UserDocument>.Update.Push(x => x.Organizations, element));
    }

    public Task DeleteUserFromOrganization(ObjectId userId, ObjectId organizationId)
    {
        return repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userId,
                Builders<UserDocument>.Update.PullFilter(x => x.Organizations,
                    x => x.OrganizationId == organizationId));
    }
}
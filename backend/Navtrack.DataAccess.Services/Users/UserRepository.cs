using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Users;

[Service(typeof(IUserRepository))]
public class UserRepository(IRepository repository) : GenericRepository<UserDocument>(repository), IUserRepository
{
    public Task<UserDocument> GetByEmailOrAppleId(string email, string id)
    {
        email = LowerAndTrim(email);

        return repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.AppleId == id || x.Email == email);
    }

    public Task<UserDocument> GetByEmailOrGoogleId(string email, string id)
    {
        email = LowerAndTrim(email);

        return repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.GoogleId == id || x.Email == email);
    }

    public Task<UserDocument> GetByEmailOrMicrosoftId(string email, string id)
    {
        email = LowerAndTrim(email);

        return repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.MicrosoftId == id || x.Email == email);
    }

    public Task DeleteAssetRoles(string assetId)
    {
        return repository.GetCollection<UserDocument>().UpdateManyAsync(
            x => x.AssetRoles.Any(y => y.AssetId == ObjectId.Parse(assetId)),
            Builders<UserDocument>.Update.PullFilter(x => x.AssetRoles,
                x => x.AssetId == ObjectId.Parse(assetId)));
    }

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

        if (updateUser.AppleId != null)
        {
            updateDefinitions.Add(
                Builders<UserDocument>.Update.Set(x => x.AppleId, updateUser.AppleId));
        }

        if (updateUser.MicrosoftId != null)
        {
            updateDefinitions.Add(
                Builders<UserDocument>.Update.Set(x => x.MicrosoftId, updateUser.MicrosoftId));
        }

        if (updateUser.GoogleId != null)
        {
            updateDefinitions.Add(
                Builders<UserDocument>.Update.Set(x => x.GoogleId, updateUser.GoogleId));
        }

        if (updateDefinitions.Count != 0)
        {
            await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == id,
                Builders<UserDocument>.Update.Combine(updateDefinitions));
        }
    }

    public Task AddAssetRole(ObjectId userId, UserAssetRoleElement userAssetRole)
    {
        return repository.GetCollection<UserDocument>()
            .UpdateOneAsync(x => x.Id == userId,
                Builders<UserDocument>.Update.AddToSet(x => x.AssetRoles, userAssetRole));
    }

    private static string LowerAndTrim(string email)
    {
        email = email.ToLower().Trim();

        return email;
    }
}
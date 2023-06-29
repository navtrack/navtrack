using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Users;

[Service(typeof(IUserDataService))]
public class UserDataService : IUserDataService
{
    private readonly IRepository repository;

    public UserDataService(IRepository repository)
    {
        this.repository = repository;
    }

    public Task<UserDocument> GetByObjectId(ObjectId id)
    {
        return repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<UserDocument> GetByEmailOrAppleId(string email, string id)
    {
        email = email.ToLower();

        return repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.AppleId == id || x.Email == email);
    }

    public Task<UserDocument> GetByEmailOrGoogleId(string email, string id)
    {
        email = email.ToLower();

        return repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.GoogleId == id || x.Email == email);
    }

    public Task<UserDocument> GetByEmailOrMicrosoftId(string email, string id)
    {
        email = email.ToLower();

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
        email = email.ToLower();

        UserDocument? user = await repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        return user;
    }

    public Task<bool> EmailIsUsed(string email)
    {
        email = email.ToLower();

        return repository.GetCollection<UserDocument>().AsQueryable()
            .AnyAsync(x => x.Email == email);
    }

    public Task<UserDocument> GetUserById(string id)
    {
        return repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(id));
    }

    public Task<List<UserDocument>> GetUsersByIds(IEnumerable<ObjectId> userIds)
    {
        return repository.GetQueryable<UserDocument>()
            .Where(x => userIds.Contains(x.Id))
            .ToListAsync();
    }

    public async Task Update(ObjectId id, UpdateUser updateUser)
    {
        List<UpdateDefinition<UserDocument>> updateDefinitions = new();

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
            updateDefinitions.Add(Builders<UserDocument>.Update.Set(x => x.Password,
                new PasswordElement
                {
                    Hash = updateUser.Password.Hash,
                    Salt = updateUser.Password.Salt
                }));
        }

        if (!string.IsNullOrEmpty(updateUser.AppleId))
        {
            updateDefinitions .Add(
                Builders<UserDocument>.Update.Set(x => x.AppleId, updateUser.AppleId));
        }
        
        if (!string.IsNullOrEmpty(updateUser.MicrosoftId))
        {
            updateDefinitions .Add(
                Builders<UserDocument>.Update.Set(x => x.AppleId, updateUser.AppleId));
        }
        
        if (!string.IsNullOrEmpty(updateUser.GoogleId))
        {
            updateDefinitions .Add(
                Builders<UserDocument>.Update.Set(x => x.AppleId, updateUser.AppleId));
        }

        if (updateDefinitions.Any())
        {
            await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == id,
                Builders<UserDocument>.Update.Combine(updateDefinitions));
        }
    }

    public Task Add(UserDocument user)
    {
        return repository.GetCollection<UserDocument>().InsertOneAsync(user);
    }
}
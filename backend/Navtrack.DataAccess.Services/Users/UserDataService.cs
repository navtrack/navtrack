using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Common;
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
        return repository.GetEntities<UserDocument>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<UserDocument> GetByAppleId(string email, string id)
    {
        return repository.GetEntities<UserDocument>().FirstOrDefaultAsync(x => x.AppleId == id || x.Email == email);
    }

    public Task<UserDocument> GetByGoogleId(string email, string id)
    {
        return repository.GetEntities<UserDocument>().FirstOrDefaultAsync(x => x.GoogleId == id || x.Email == email);
    }

    public Task<UserDocument> GetByMicrosoftId(string email, string id)
    {
        return repository.GetEntities<UserDocument>().FirstOrDefaultAsync(x => x.MicrosoftId == id || x.Email == email);
    }

    public Task DeleteAssetRoles(string assetId)
    {
        return repository.GetCollection<UserDocument>().UpdateManyAsync(
            x => x.AssetRoles.Any(y => y.AssetId == ObjectId.Parse(assetId)),
            Builders<UserDocument>.Update.PullFilter(x => x.AssetRoles,
                x => x.AssetId == ObjectId.Parse(assetId)));
    }

    public async Task<UserDocument?> GetUserByEmail(string email)
    {
        email = email.ToLower();

        UserDocument? user = await repository.GetEntities<UserDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        return user;
    }

    public Task<bool> EmailExists(string email)
    {
        email = email.ToLower();

        return repository.GetEntities<UserDocument>()
            .AnyAsync(x => x.Email == email);
    }

    public Task<UserDocument> GetUserById(string id)
    {
        return repository.GetEntities<UserDocument>()
            .FirstOrDefaultAsync(x => x.Id == ObjectId.Parse(id));
    }

    public Task<List<UserDocument>> GetUsersByIds(IEnumerable<ObjectId> userIds)
    {
        return repository.GetEntities<UserDocument>()
            .Where(x => userIds.Contains(x.Id))
            .ToListAsync();
    }

    public Task ChangePassword(ObjectId id, string hash, string salt)
    {
        return repository.GetCollection<UserDocument>().UpdateOneAsync(
            x => x.Id == id,
            Builders<UserDocument>.Update.Set(x => x.Password,
                new PasswordElement
                {
                    Hash = hash,
                    Salt = salt
                }));
    }

    public async Task UpdateUser(UserDocument currentUser, string email, UnitsType? unitsType = null)
    {
        List<UpdateDefinition<UserDocument>> updateDefinitions = new();

        if (!string.IsNullOrEmpty(email) &&
            !string.Equals(currentUser.Email, email, StringComparison.CurrentCultureIgnoreCase))
        {
            updateDefinitions.Add(Builders<UserDocument>.Update.Set(x => x.Email, email.ToLower()));
        }

        if (unitsType.HasValue && unitsType.Value != currentUser.UnitsType)
        {
            updateDefinitions.Add(Builders<UserDocument>.Update.Set(x => x.UnitsType, unitsType.Value));
        }

        if (updateDefinitions.Any())
        {
            await repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == currentUser.Id,
                Builders<UserDocument>.Update.Combine(updateDefinitions));
        }
    }

    public Task Add(UserDocument user)
    {
        return repository.GetCollection<UserDocument>().InsertOneAsync(user);
    }

    public Task SetGoogleId(ObjectId userDocumentId, string id)
    {
        return repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == userDocumentId,
            Builders<UserDocument>.Update.Set(x => x.GoogleId, id));
    }

    public Task SetMicrosoftId(ObjectId userDocumentId, string id)
    {
        return repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == userDocumentId,
            Builders<UserDocument>.Update.Set(x => x.MicrosoftId, id));
    }

    public Task SetAppleId(ObjectId userDocumentId, string id)
    {
        return repository.GetCollection<UserDocument>().UpdateOneAsync(x => x.Id == userDocumentId,
            Builders<UserDocument>.Update.Set(x => x.AppleId, id));
    }
}
using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Users;

[Service(typeof(IPasswordResetDataService))]
public class PasswordResetDataService : IPasswordResetDataService
{
    private readonly IRepository repository;

    public PasswordResetDataService(IRepository repository)
    {
        this.repository = repository;
    }

    public Task<int> GetCountOfPasswordResets(string ipAddress, DateTime fromDate)
    {
        return repository.GetEntities<PasswordResetDocument>()
            .Where(x => x.IpAddress == ipAddress && x.Created.Date > fromDate)
            .CountAsync();
    }

    public Task Add(PasswordResetDocument document)
    {
        return repository.GetCollection<PasswordResetDocument>().InsertOneAsync(document);
    }

    public async Task<PasswordResetDocument?> GetLatestFromHash(string hash)
    {
        PasswordResetDocument? documentByHash = await repository.GetEntities<PasswordResetDocument>().FirstOrDefaultAsync(x => x.Hash == hash);

        if (documentByHash != null)
        {
            PasswordResetDocument? latestUserDocument = await repository.GetEntities<PasswordResetDocument>()
                .Where(x => x.UserId == documentByHash.UserId)
                .OrderByDescending(x => x.Created.Date)
                .FirstOrDefaultAsync();

            return latestUserDocument;
        }

        return null;
    }

    public Task MarkAsInvalid(ObjectId id)
    {
        return repository.GetCollection<PasswordResetDocument>().UpdateOneAsync(x => x.Id == id,
            Builders<PasswordResetDocument>.Update.Set(x => x.Invalid, true));
    }

    public Task MarkAsInvalidByUserId(ObjectId userId)
    {
        return repository.GetCollection<PasswordResetDocument>().UpdateManyAsync(x => x.UserId == userId,
            Builders<PasswordResetDocument>.Update.Set(x => x.Invalid, true));
    }
}
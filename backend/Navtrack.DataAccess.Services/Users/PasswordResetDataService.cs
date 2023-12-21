using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Users;

[Service(typeof(IPasswordResetRepository))]
public class PasswordResetRepository(IRepository repository)
    : GenericRepository<PasswordResetDocument>(repository), IPasswordResetRepository
{
    public Task<int> GetCountOfPasswordResets(string ipAddress, string email, DateTime fromDate)
    {
        return repository.GetQueryable<PasswordResetDocument>()
            .Where(x => x.IpAddress == ipAddress && x.Created.Date > fromDate && x.Email == email)
            .CountAsync();
    }

    public async Task<PasswordResetDocument?> GetLatestFromHash(string hash)
    {
        PasswordResetDocument? documentByHash = await repository.GetQueryable<PasswordResetDocument>().FirstOrDefaultAsync(x => x.Hash == hash);

        if (documentByHash != null)
        {
            PasswordResetDocument? latestUserDocument = await repository.GetQueryable<PasswordResetDocument>()
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
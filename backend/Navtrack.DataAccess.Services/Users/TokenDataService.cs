using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Users;

[Service(typeof(ITokenDataService))]
public class TokenDataService : ITokenDataService
{
    private readonly IRepository repository;

    public TokenDataService(IRepository repository)
    {
        this.repository = repository;
    }

    public Task Add(RefreshTokenDocument document)
    {
        return repository.GetCollection<RefreshTokenDocument>()
            .ReplaceOneAsync(x => x.Id == document.Id, document, new ReplaceOptions
            {
                IsUpsert = true,
            });
    }

    public Task Remove(string userId)
    {
        return repository.GetCollection<RefreshTokenDocument>().DeleteOneAsync(x => x.Id == ObjectId.Parse(userId));
    }

    public async Task<RefreshTokenDocument?> GetByUserId(string userId)
    {
        return ObjectId.TryParse(userId, out ObjectId objectId)
            ? await repository.GetEntities<RefreshTokenDocument>().FirstOrDefaultAsync(x => x.Id == objectId)
            : null;
    }
}
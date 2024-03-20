using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.DataAccess.Model.Users.RefreshTokens;
using Navtrack.DataAccess.Mongo;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Users;

[Service(typeof(IRefreshTokenRepository))]
public class RefreshTokenRepository(IRepository repository) : IRefreshTokenRepository
{
    public Task Add(RefreshTokenDocument document)
    {
        return repository.GetCollection<RefreshTokenDocument>()
            .ReplaceOneAsync(x => x.JwtId == document.JwtId, document, new ReplaceOptions
            {
                IsUpsert = true
            });
    }

    public Task Remove(string subjectId, string clientId)
    {
        return repository.GetCollection<RefreshTokenDocument>()
            .DeleteOneAsync(x => x.AccessToken.SubjectId == ObjectId.Parse(subjectId) && x.AccessToken.ClientId == clientId);
    }

    public Task<RefreshTokenDocument>? Get(string refreshTokenHandle)
    {
        return repository.GetQueryable<RefreshTokenDocument>()
            .FirstOrDefaultAsync(x => x.Hash == refreshTokenHandle);
    }

    public Task Remove(string refreshTokenHandle)
    {
        return repository.GetCollection<RefreshTokenDocument>()
            .DeleteOneAsync(x => x.Hash == refreshTokenHandle);
    }
}
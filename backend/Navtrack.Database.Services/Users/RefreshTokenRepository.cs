using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Authentication;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Users;

[Service(typeof(IRefreshTokenRepository))]
public class RefreshTokenRepository(IPostgresRepository repository) : IRefreshTokenRepository
{
    public async Task Add(AuthRefreshTokenEntity entity)
    {
        await repository.GetQueryable<AuthRefreshTokenEntity>()
            .Where(x => x.JwtId == entity.JwtId)
            .ExecuteDeleteAsync();

        repository.GetQueryable<AuthRefreshTokenEntity>()
            .Add(entity);

        await repository.GetDbContext().SaveChangesAsync();
    }

    public Task Remove(string subjectId, string clientId)
    {
        return repository.GetQueryable<AuthRefreshTokenEntity>()
            .Where(x => x.SubjectId == subjectId && x.ClientId == clientId)
            .ExecuteDeleteAsync();
        // .DeleteOneAsync(x => x.AccessToken.SubjectId == ObjectId.Parse(subjectId) && x.AccessToken.ClientId == clientId);
    }

    public Task<AuthRefreshTokenEntity?> Get(string refreshTokenHandle)
    {
        return repository.GetQueryable<AuthRefreshTokenEntity>()
            .FirstOrDefaultAsync(x => x.Hash == refreshTokenHandle);
    }

    public Task Remove(string refreshTokenHandle)
    {
        return repository.GetQueryable<AuthRefreshTokenEntity>()
            .Where(x => x.Hash == refreshTokenHandle)
            .ExecuteDeleteAsync();
    }
}
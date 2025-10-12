using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Users;

[Service(typeof(IPasswordResetRepository))]
public class PasswordResetRepository(IPostgresRepository repository)
    : GenericPostgresRepository<UserPasswordResetEntity>(repository), IPasswordResetRepository
{
    public Task<int> GetCountOfPasswordResets(string ipAddress, string email, DateTime fromDate)
    {
        return repository.GetQueryable<UserPasswordResetEntity>()
            .Where(x => x.IpAddress == ipAddress && x.CreatedDate > fromDate && x.Email == email)
            .CountAsync();
    }

    public async Task<UserPasswordResetEntity?> GetLatestFromId(string id)
    {
        UserPasswordResetEntity? documentByHash =
            await repository.GetQueryable<UserPasswordResetEntity>().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

        if (documentByHash != null)
        {
            UserPasswordResetEntity? latestUserDocument = await repository.GetQueryable<UserPasswordResetEntity>()
                .Where(x => x.CreatedBy == documentByHash.CreatedBy)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            return latestUserDocument;
        }

        return null;
    }

    public Task MarkAsInvalidByUserId(Guid userId)
    {
        return repository.GetQueryable<UserPasswordResetEntity>()
            .Where(x => x.CreatedBy == userId)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Invalid, true));
    }
}
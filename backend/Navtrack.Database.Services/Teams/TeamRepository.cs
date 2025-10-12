using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Teams;

[Service(typeof(ITeamRepository))]
public class TeamRepository(IPostgresRepository repository)
    : GenericPostgresRepository<TeamEntity>(repository), ITeamRepository
{
    public Task<bool> NameIsUsed(string name, Guid organizationId, Guid? teamId = null)
    {
        Expression<Func<TeamEntity, bool>> filter = teamId == null
            ? x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower())
            : x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower()) &&
                x.Id != teamId;

        return repository.GetQueryable<TeamEntity>()
            .AnyAsync(filter);
    }

    public Task<List<TeamEntity>> GetByOrganizationId(Guid organizationId)
    {
        return repository.GetQueryable<TeamEntity>()
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task AddAsset(Guid teamId, Guid assetId, Guid userId)
    {
        TeamAssetEntity teamAsset = new()
        {
            TeamId = teamId,
            AssetId = assetId,
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow
        };

        repository.GetQueryable<TeamAssetEntity>().Add(teamAsset);
        await repository.GetDbContext().SaveChangesAsync();

        await UpdateAssetsCount(teamId);
    }

    public async Task UpdateAssetsCount(Guid teamId)
    {
        int assetsCount = await repository.GetQueryable<TeamAssetEntity>()
            .CountAsync(x => x.TeamId == teamId);

        await repository.GetQueryable<TeamEntity>()
            .Where(x => x.Id == teamId)
            .ExecuteUpdateAsync(x => x.SetProperty(t => t.AssetsCount, assetsCount));
    }

    public async Task UpdateUsersCount(Guid teamId)
    {
        int usersCount = await repository.GetQueryable<TeamUserEntity>()
            .CountAsync(x => x.TeamId == teamId);

        await repository.GetQueryable<TeamEntity>()
            .Where(x => x.Id == teamId)
            .ExecuteUpdateAsync(x => x.SetProperty(t => t.UsersCount, usersCount));
    }
}
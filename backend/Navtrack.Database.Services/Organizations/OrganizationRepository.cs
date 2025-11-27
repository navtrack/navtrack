using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Shared;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Organizations;

[Service(typeof(IOrganizationRepository))]
public class OrganizationRepository(IPostgresRepository repository)
    : GenericPostgresRepository<OrganizationEntity>(repository), IOrganizationRepository
{
    public override Task<List<OrganizationEntity>> GetByIds(IEnumerable<Guid> ids)
    {
        return repository.GetQueryable<OrganizationEntity>()
            .Where(x => ids.Contains(x.Id))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public Task UpdateName(string organizationId, string name)
    {
        return repository.GetQueryable<OrganizationEntity>()
            .Where(x => x.Id == Guid.Parse(organizationId))
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Name, name));
    }

    public Task UpdateAssetsCount(Guid organizationId)
    {
        int assets = repository.GetQueryable<AssetEntity>()
            .Count(x => x.OrganizationId == organizationId);

        return repository.GetQueryable<OrganizationEntity>()
            .Where(x => x.Id == organizationId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.AssetsCount, assets));
    }

    public Task UpdateUsersCount(Guid organizationId)
    {
        int users = repository.GetQueryable<UserEntity>()
            .Count(x => x.OrganizationUsers.Any(y => y.OrganizationId == organizationId));

        return repository.GetQueryable<OrganizationEntity>()
            .Where(x => x.Id == organizationId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.UsersCount, users));
    }

    public Task UpdateTeamsCount(Guid organizationId)
    {
        int teams = repository.GetQueryable<TeamEntity>()
            .Count(x => x.OrganizationId == organizationId);

        return repository.GetQueryable<OrganizationEntity>()
            .Where(x => x.Id == organizationId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.TeamsCount, teams));
    }

    public Task<List<OrganizationUserEntity>> GetUsers(Guid organizationId)
    {
        return repository.GetQueryable<OrganizationUserEntity>()
            .Include(x => x.User)
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.User.Email)
            .ToListAsync();
    }

    public Task UpdateWorkSchedules(string organizationId, WorkScheduleEntity workSchedule)
    {
        return repository.GetQueryable<OrganizationEntity>()
            .Where(x => x.Id == Guid.Parse(organizationId))
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.WorkSchedule, workSchedule));
    }
}
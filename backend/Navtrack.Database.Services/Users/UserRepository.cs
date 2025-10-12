using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Users;

[Service(typeof(IUserRepository))]
public class UserRepository(IPostgresRepository repository)
    : GenericPostgresRepository<UserEntity>(repository), IUserRepository
{
    public override Task<UserEntity?> GetById(string? id)
    {
        return !string.IsNullOrEmpty(id)
            ? repository.GetQueryable<UserEntity>()
                .Include(x => x.Organizations)
                .Include(x => x.OrganizationUsers)
                .Include(x => x.Teams)
                .Include(x => x.TeamUsers)
                .Include(x => x.Assets)
                .Include(x => x.AssetUsers)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id))
            : Task.FromResult<UserEntity?>(null);
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        email = LowerAndTrim(email);

        UserEntity? user = await repository.GetQueryable<UserEntity>()
            .FirstOrDefaultAsync(x => x.Email == email);

        return user;
    }

    public Task<bool> EmailIsUsed(string email)
    {
        email = LowerAndTrim(email);

        return repository.GetQueryable<UserEntity>().AnyAsync(x => x.Email == email);
    }

    public async Task Update(Guid id, UpdateUser updateUser)
    {
        // TODO optimize multiple updates into one with EF Core 10
        if (!string.IsNullOrEmpty(updateUser.Email))
        {
            await repository.GetQueryable<UserEntity>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.Email, updateUser.Email.ToLower()));
        }

        if (updateUser.UnitsType.HasValue)
        {
            await repository.GetQueryable<UserEntity>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.UnitsType, updateUser.UnitsType.Value));
        }

        if (updateUser.Password != null)
        {
            await repository.GetQueryable<UserEntity>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.PasswordSalt, updateUser.Password.Salt)
                    .SetProperty(u => u.PasswordHash, updateUser.Password.Hash));
        }
    }

    public Task<List<UserEntity>> GetByOrganizationId(Guid organizationId)
    {
        return repository.GetQueryable<UserEntity>()
            .Where(x => x.OrganizationUsers.Any(y => y.OrganizationId == organizationId))
            .OrderBy(x => x.Email)
            .ToListAsync();
    }

    public Task<int> GetOrganizationOwnersCount(Guid organizationId)
    {
        return repository.GetQueryable<OrganizationUserEntity>()
            .CountAsync(x => x.OrganizationId == organizationId && x.UserRole == OrganizationUserRole.Owner);
    }

    public Task<List<UserEntity>> GetByTeamId(Guid teamId)
    {
        return repository.GetQueryable<UserEntity>()
            .Where(x => x.Teams.Any(y => y.Id == teamId))
            .OrderBy(x => x.Email)
            .ToListAsync();
    }

    public Task<List<UserEntity>> GetByAssetId(Guid assetId)
    {
        return repository.GetQueryable<UserEntity>()
            .Where(x => x.Assets.Any(y => y.Id == assetId))
            .OrderBy(x => x.Email)
            .ToListAsync();
    }

    public Task RemoveAssetFromUsers(Guid assetId)
    {
        return repository.GetQueryable<AssetUserEntity>()
            .Where(x => x.AssetId == assetId)
            .ExecuteDeleteAsync();
    }

    private static string LowerAndTrim(string email)
    {
        email = email.ToLower().Trim();

        return email;
    }

    public Task AddAssetToUser(Guid userId, AssetUserEntity asset)
    {
        repository.GetQueryable<AssetUserEntity>()
            .Add(new AssetUserEntity
            {
                UserId = userId,
                AssetId = asset.AssetId,
                UserRole = asset.UserRole,
                CreatedBy = asset.CreatedBy,
                CreatedDate = asset.CreatedDate
            });

        return repository.GetDbContext().SaveChangesAsync();
    }

    public Task RemoveAssetFromUser(Guid assetId, Guid userId)
    {
        return repository.GetQueryable<AssetUserEntity>()
            .Where(x => x.UserId == userId && x.AssetId == assetId)
            .ExecuteDeleteAsync();
    }

    public async Task AddUserToTeam(TeamUserEntity element)
    {
        repository.GetQueryable<TeamUserEntity>().Add(element);

        await repository.GetDbContext().SaveChangesAsync();

        await repository.GetQueryable<TeamEntity>()
            .Where(x => x.Id == element.TeamId)
            .ExecuteUpdateAsync(x => x.SetProperty(t => t.UsersCount, t => t.UsersCount + 1));
    }

    public Task UpdateTeamUser(Guid teamId, Guid userId, TeamUserRole userRole)
    {
        return repository.GetQueryable<TeamUserEntity>()
            .Where(x => x.TeamId == teamId && x.UserId == userId)
            .ExecuteUpdateAsync(x => x.SetProperty(t => t.UserRole, userRole));
    }

    public Task RemoveTeamFromUser(Guid userId, Guid teamId)
    {
        return repository.GetQueryable<TeamUserEntity>()
            .Where(x => x.UserId == userId && x.TeamId == teamId)
            .ExecuteDeleteAsync();
    }

    public Task RemoveTeamFromUsers(Guid teamId)
    {
        return repository.GetQueryable<TeamUserEntity>()
            .Where(x => x.TeamId == teamId)
            .ExecuteDeleteAsync();
    }

    public Task UpdateOrganizationUser(Guid userId, Guid organizationId, OrganizationUserRole userRole)
    {
        return repository.GetQueryable<OrganizationUserEntity>()
            .Where(x => x.OrganizationId == organizationId && x.UserId == userId)
            .ExecuteUpdateAsync(x => x.SetProperty(t => t.UserRole, userRole));
    }

    public async Task AddUserToOrganization(OrganizationUserEntity entity)
    {
        repository.GetQueryable<OrganizationUserEntity>().Add(entity);

        await repository.GetDbContext().SaveChangesAsync();
    }

    public Task DeleteUserFromOrganization(Guid userId, Guid organizationId)
    {
        return repository.GetQueryable<OrganizationUserEntity>()
            .Where(x => x.OrganizationId == organizationId && x.UserId == userId)
            .ExecuteDeleteAsync();
    }
}
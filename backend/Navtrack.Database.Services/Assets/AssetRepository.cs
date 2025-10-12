using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Assets;

[Service(typeof(IAssetRepository))]
public class AssetRepository(IPostgresRepository repository)
    : GenericPostgresRepository<AssetEntity>(repository), IAssetRepository
{
    public override Task<AssetEntity?> GetById(string? id)
    {
        return !string.IsNullOrEmpty(id)
            ? repository.GetQueryable<AssetEntity>()
                .Include(x => x.Device)
                .Include(x => x.LastMessage)
                .Include(x => x.LastPositionMessage)
                .Include(x => x.Organization)
                .Include(x => x.Teams)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id))
            : Task.FromResult<AssetEntity?>(null);
    }


    public Task<AssetEntity?> Get(string serialNumber, int protocolPort)
    {
        Task<AssetEntity?> asset = repository.GetQueryable<AssetEntity>()
            .Include(x => x.Device)
            .FirstOrDefaultAsync(x =>
                x.Device != null &&
                x.Device.SerialNumber == serialNumber &&
                x.Device.ProtocolPort == protocolPort);

        return asset;
    }

    public Task<bool> NameIsUsed(Guid organizationId, string name, Guid? assetId = null)
    {
        Expression<Func<AssetEntity, bool>> filter = assetId == null
            ? x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower())
            : x =>
                x.OrganizationId == organizationId &&
                x.Name.ToLower().Equals(name.ToLower()) &&
                x.Id != assetId;

        return repository.GetQueryable<AssetEntity>().AnyAsync(filter);
    }

    public Task SetActiveDevice(Guid assetId, Guid deviceId)
    {
        return repository.GetQueryable<AssetEntity>()
            .Where(x => x.Id == assetId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.DeviceId, deviceId));
    }

    public async Task UpdateMessages(Guid assetId, Guid lastMessageId,
        Guid? positionMessageId)
    {
        if (positionMessageId != null)
        {
            await repository.GetQueryable<AssetEntity>()
                .Where(x => x.Id == assetId)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(y => y.LastMessageId, lastMessageId)
                    .SetProperty(y => y.LastPositionMessageId, positionMessageId));
        }
        else
        {
            await repository.GetQueryable<AssetEntity>()
                .Where(x => x.Id == assetId)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(y => y.LastMessageId, lastMessageId)); 
        }

    }

    public Task<List<TeamAssetEntity>> GetByTeamId(Guid teamId)
    {
        return repository.GetQueryable<TeamAssetEntity>()
            .Include(x => x.Asset)
            .Where(x => x.TeamId == teamId)
            .OrderBy(x => x.Team.Name)
            .ToListAsync();
    }

    public Task<List<AssetEntity>> GetByOrganizationId(Guid organizationId)
    {
        return repository.GetQueryable<AssetEntity>()
            .Include(x => x.Device)
            .Include(x => x.LastMessage)
            .Include(x => x.LastPositionMessage)
            .Where(x => x.OrganizationId == organizationId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public Task<List<AssetEntity>> GetByAssetAndTeamIds(List<Guid> assetIds, List<Guid> teamIds)
    {
        return repository.GetQueryable<AssetEntity>()
            .Include(x => x.Device)
            .Include(x => x.LastMessage)
            .Include(x => x.LastPositionMessage)
            .Where(x => assetIds.Contains(x.Id) || x.Teams.Any(y => teamIds.Contains(y.Id)))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<AssetEntity>> GetByUserAndOrganizationId(Guid userId, Guid organizationId)
    {
        UserEntity user = await repository.GetQueryable<UserEntity>().FirstAsync(x => x.Id == userId);

        List<Guid> assetIds =
            user.Assets.Where(x => x.OrganizationId == organizationId).Select(x => x.Id).ToList();

        List<Guid> teamIds =
            user.Teams.Where(x => x.OrganizationId == organizationId).Select(x => x.Id).ToList();

        List<AssetEntity> assets = await repository.GetQueryable<AssetEntity>()
            .Include(x => x.Device)
            .Include(x => x.LastMessage)
            .Include(x => x.LastPositionMessage)
            .Where(x => x.OrganizationId == organizationId &&
                        (assetIds.Contains(x.Id) || x.Teams.Any(y => teamIds.Contains(y.Id))))
            .OrderBy(x => x.Name)
            .ToListAsync();

        return assets;
    }

    public Task RemoveAssetFromTeam(Guid teamId, Guid assetId)
    {
        return repository.GetQueryable<TeamAssetEntity>()
            .Where(x => x.TeamId == teamId && x.AssetId == assetId)
            .ExecuteDeleteAsync();
    }

    public Task RemoveTeamFromAssets(Guid teamId)
    {
        return repository.GetQueryable<TeamAssetEntity>()
            .Where(x => x.TeamId == teamId)
            .ExecuteDeleteAsync();
    }

    public Task<List<AssetUserEntity>> GetUsers(Guid assetId)
    {
        return repository.GetQueryable<AssetUserEntity>().Where(x => x.AssetId == assetId)
            .Include(x => x.User)
            .OrderBy(x => x.User.Email)
            .ToListAsync();
    }

    public Task<List<AssetEntity>> GetAssetsByAssetAndTeamIds(List<Guid> assetIds, List<Guid> teamIds)
    {
        return repository.GetQueryable<AssetEntity>()
            .Include(x => x.Device)
            .Include(x => x.LastMessage)
            .Include(x => x.LastPositionMessage)
            .Where(x => assetIds.Contains(x.Id) || x.Teams.Any(y => teamIds.Contains(y.Id)))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}
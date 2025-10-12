using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Assets;

public interface IAssetRepository : IGenericPostgresRepository<AssetEntity>
{
    Task<AssetEntity?> Get(string serialNumber, int protocolPort);
    Task<bool> NameIsUsed(Guid organizationId, string name, Guid? assetId = null);
    Task UpdateMessages(Guid assetId, Guid lastMessageId, Guid? positionMessageId);
    Task SetActiveDevice(Guid assetId, Guid device);
    Task<List<TeamAssetEntity>> GetByTeamId(Guid teamId);
    Task<List<AssetEntity>> GetByOrganizationId(Guid organizationId);
    Task<List<AssetEntity>> GetByAssetAndTeamIds(List<Guid> assetIds, List<Guid> teamIds);
    Task RemoveAssetFromTeam(Guid teamId, Guid assetId);
    Task RemoveTeamFromAssets(Guid teamId);
    Task<List<AssetUserEntity>> GetUsers(Guid assetId);
    Task<List<AssetEntity>> GetAssetsByAssetAndTeamIds(List<Guid> assetIds, List<Guid> teamIds);
}
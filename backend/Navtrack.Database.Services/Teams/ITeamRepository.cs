using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Teams;

public interface ITeamRepository : IGenericPostgresRepository<TeamEntity>
{
    Task<bool> NameIsUsed(string name, Guid organizationId, Guid? teamId = null);
    Task<List<TeamEntity>> GetByOrganizationId(Guid organizationId);
    Task UpdateUsersCount(Guid teamId);
    Task UpdateAssetsCount(Guid teamId);
    Task AddAsset(Guid teamId, Guid assetId, Guid userId);
}
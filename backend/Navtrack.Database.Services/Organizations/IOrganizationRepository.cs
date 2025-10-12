using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Organizations;

public interface IOrganizationRepository : IGenericPostgresRepository<OrganizationEntity>
{
    Task UpdateName(string organizationId, string name);
    Task UpdateAssetsCount(Guid organizationId);
    Task UpdateUsersCount(Guid organizationId);
    Task UpdateTeamsCount(Guid organizationId);
    Task<List<OrganizationUserEntity>> GetUsers(Guid organizationId);
}
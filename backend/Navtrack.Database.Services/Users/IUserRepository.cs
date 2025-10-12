using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Users;

public interface IUserRepository : IGenericPostgresRepository<UserEntity> 
{
    Task<UserEntity?> GetByEmail(string email);
    Task<bool> EmailIsUsed(string email);
    Task Update(Guid id, UpdateUser updateUser);
    
    Task<List<UserEntity>> GetByOrganizationId(Guid organizationId);

    Task<int> GetOrganizationOwnersCount(Guid organizationId);
    Task<List<UserEntity>> GetByTeamId(Guid teamId);
    Task<List<UserEntity>> GetByAssetId(Guid assetId);
    
    Task AddAssetToUser(Guid userId, AssetUserEntity asset);
    Task RemoveAssetFromUser(Guid assetId, Guid userId);
    Task RemoveAssetFromUsers(Guid assetId);
    
    Task AddUserToTeam(TeamUserEntity element);
    Task UpdateTeamUser(Guid teamId, Guid userId, TeamUserRole userRole);
    Task RemoveTeamFromUser(Guid userId, Guid teamId);
    Task RemoveTeamFromUsers(Guid teamId);
    
    Task AddUserToOrganization(OrganizationUserEntity entity);
    Task UpdateOrganizationUser(Guid userId, Guid organizationId, OrganizationUserRole userRole);
    Task DeleteUserFromOrganization(Guid userId, Guid organizationId);
}
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Users;

public interface IUserRepository : IGenericRepository<UserDocument> 
{
    Task<UserDocument?> GetByEmail(string email);
    Task<bool> EmailIsUsed(string email);
    Task Update(ObjectId id, UpdateUser updateUser);
    
    Task<List<UserDocument>> GetByOrganizationId(ObjectId organizationId);

    Task<int> GetOrganizationOwnersCount(ObjectId organizationId);
    Task<List<UserDocument>> GetByTeamId(string teamId);
    Task<List<UserDocument>> GetByAssetId(ObjectId assetId);
    
    Task AddAssetToUser(ObjectId userId, UserAssetElement asset);
    Task RemoveAssetFromUser(ObjectId assetId, ObjectId userId);
    Task RemoveAssetFromUsers(ObjectId assetId);
    
    Task AddUserToTeam(ObjectId userId, UserTeamElement element);
    Task UpdateTeamUser(ObjectId teamId, ObjectId userId, TeamUserRole userRole);
    Task RemoveTeamFromUser(ObjectId userId, ObjectId teamId);
    Task RemoveTeamFromUsers(ObjectId teamId);
    
    Task AddUserToOrganization(ObjectId userId, UserOrganizationElement element);
    Task UpdateOrganizationUser(ObjectId userId, ObjectId organizationId, OrganizationUserRole userRole);
    Task DeleteUserFromOrganization(ObjectId userId, ObjectId organizationId);
}
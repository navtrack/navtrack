using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Teams;

public interface ITeamRepository : IGenericRepository<TeamDocument>
{
    Task<bool> NameIsUsed(string name, ObjectId organizationId, ObjectId? teamId = null);
    Task UpdateName(ObjectId teamId, string name);
    Task<List<TeamDocument>> GetByOrganizationId(ObjectId organizationId);
  
    Task UpdateUsersCount(ObjectId teamId);
    Task UpdateAssetsCount(ObjectId teamId);
}
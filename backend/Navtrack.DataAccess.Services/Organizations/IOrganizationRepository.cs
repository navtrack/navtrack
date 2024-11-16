using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Organizations;

public interface IOrganizationRepository : IGenericRepository<OrganizationDocument>
{
    Task UpdateName(string assetId, string name);
    Task UpdateAssetsCount(ObjectId organizationId);
    Task UpdateUsersCount(ObjectId organizationId);
    Task UpdateTeamsCount(ObjectId organizationId);
}
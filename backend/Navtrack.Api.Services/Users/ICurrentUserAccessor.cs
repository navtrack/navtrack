using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Users;

public interface ICurrentUserAccessor
{
    ObjectId GetId();
    Task<UserDocument> GetCurrentUser();
}
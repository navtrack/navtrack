using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Users;

public interface IUserDataService
{
    Task<UserDocument> GetById(ObjectId id);
    Task DeleteAssetRoles(string assetId);
    Task<UserDocument> GetUserByEmail(string email);
    Task<bool> EmailIsUsed(string email);
    Task<UserDocument> GetUserById(string id);
    Task<List<UserDocument>> GetUsersByIds(IEnumerable<ObjectId> userIds);
    Task ChangePassword(ObjectId id, string hash, string salt);
    Task UpdateUser(UserDocument currentUser, string email, UnitsType? unitsType);
    Task Add(UserDocument user);
}
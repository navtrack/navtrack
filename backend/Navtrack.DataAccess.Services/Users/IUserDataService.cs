using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Users;

public interface IUserDataService
{
    Task<UserDocument> GetByObjectId(ObjectId id);
    Task<UserDocument> GetByEmailOrAppleId(string email, string id);
    Task<UserDocument> GetByEmailOrGoogleId(string email, string id);
    Task<UserDocument> GetByEmailOrMicrosoftId(string email, string id);
    Task DeleteAssetRoles(string assetId);
    Task<UserDocument?> GetByEmail(string email);
    Task<bool> EmailIsUsed(string email);
    Task<List<UserDocument>> GetUsersByIds(IEnumerable<ObjectId> userIds);
    Task Update(ObjectId id, UpdateUser updateUser);
    Task Add(UserDocument user);
}
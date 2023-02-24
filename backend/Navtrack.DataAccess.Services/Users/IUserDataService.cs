using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Users;

public interface IUserDataService
{
    Task<UserDocument> GetByObjectId(ObjectId id);
    Task<UserDocument> GetByAppleId(string email, string id);
    Task<UserDocument> GetByGoogleId(string email, string id);
    Task<UserDocument> GetByMicrosoftId(string email, string id);
    Task DeleteAssetRoles(string assetId);
    Task<UserDocument?> GetUserByEmail(string email);
    Task<bool> EmailExists(string email);
    Task<List<UserDocument>> GetUsersByIds(IEnumerable<ObjectId> userIds);
    Task ChangePassword(ObjectId id, string hash, string salt);
    Task UpdateUser(UserDocument currentUser, string email, UnitsType? unitsType = null);
    Task Add(UserDocument user);
    Task SetGoogleId(ObjectId userDocumentId, string id);
    Task SetMicrosoftId(ObjectId userDocumentId, string id);
    Task SetAppleId(ObjectId userDocumentId, string id);
}
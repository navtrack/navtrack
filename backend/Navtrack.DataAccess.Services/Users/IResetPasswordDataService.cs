using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Users;

public interface IPasswordResetRepository : IGenericRepository<PasswordResetDocument>
{
    Task<int> GetCountOfPasswordResets(string ipAddress, DateTime fromDate);
    Task<PasswordResetDocument?> GetLatestFromHash(string hash);
    Task MarkAsInvalid(ObjectId id);
    Task MarkAsInvalidByUserId(ObjectId userId);
}
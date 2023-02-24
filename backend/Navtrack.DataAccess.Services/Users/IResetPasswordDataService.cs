using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Users;

public interface IPasswordResetDataService
{
    Task<int> GetCountOfPasswordResets(string ipAddress, DateTime fromDate);
    Task Add(PasswordResetDocument document);
    Task<PasswordResetDocument?> GetLatestFromHash(string hash);
    Task MarkAsInvalid(ObjectId id);
    Task MarkAsInvalidByUserId(ObjectId userId);
}
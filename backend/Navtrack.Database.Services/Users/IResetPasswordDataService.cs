using System;
using System.Threading.Tasks;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Services.Users;

public interface IPasswordResetRepository : IGenericPostgresRepository<UserPasswordResetEntity>
{
    Task<int> GetCountOfPasswordResets(string ipAddress, string email, DateTime fromDate);
    Task<UserPasswordResetEntity?> GetLatestFromId(string id);
    Task MarkAsInvalidByUserId(Guid userId);
}
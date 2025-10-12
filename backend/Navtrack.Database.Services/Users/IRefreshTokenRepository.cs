using System.Threading.Tasks;
using Navtrack.Database.Model.Authentication;

namespace Navtrack.Database.Services.Users;

public interface IRefreshTokenRepository
{
    Task Add(AuthRefreshTokenEntity entity);
    Task Remove(string subjectId, string clientId);
    Task<AuthRefreshTokenEntity?> Get(string refreshTokenHandle);
    Task Remove(string refreshTokenHandle);
}
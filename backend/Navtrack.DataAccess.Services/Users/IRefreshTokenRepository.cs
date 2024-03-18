using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Model.Users.RefreshTokens;

namespace Navtrack.DataAccess.Services.Users;

public interface IRefreshTokenRepository
{
    Task Add(RefreshTokenDocument document);
    Task Remove(string subjectId, string clientId);
    Task<RefreshTokenDocument>? Get(string refreshTokenHandle);
    Task Remove(string refreshTokenHandle);
}
using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Users;

public interface IRefreshTokenDataService
{
    Task Add(RefreshTokenDocument document);
    Task Remove(string subjectId, string clientId);
    Task<RefreshTokenDocument>? Get(string refreshTokenHandle);
    Task Remove(string refreshTokenHandle);
}
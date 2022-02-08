using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.DataAccess.Services.Users;

public interface ITokenDataService
{
    Task Add(RefreshTokenDocument document);
    Task Remove(string userId);
    Task<RefreshTokenDocument?> GetByUserId(string userId);
}
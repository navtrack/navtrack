using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Custom;

namespace Navtrack.DataAccess.Services
{
    public interface IAssetDataService
    {
        Task<bool> UserHasRole(int userId, int assetId, UserAssetRole userAssetRole);
        Task<bool> HasActiveDeviceId(int assetId, string deviceId);
    }
}
using System.Threading.Tasks;
using Navtrack.DataAccess.Model.Custom;

namespace Navtrack.DataAccess.Services
{
    public interface IAssetDataService
    {
        Task<bool> UserHasRoleForAsset(int userId, UserAssetRole userAssetRole, int assetId);
        Task<bool> UserHasRolesForAsset(int userId, UserAssetRole[] userAssetRoles, int assetId);
        Task<bool> UserHasRoleForDevice(int userId, UserAssetRole userAssetRole, int deviceId);
        Task<bool> HasActiveDeviceId(int assetId, string deviceId);
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services
{
    [Service(typeof(IAssetDataService))]
    public class AssetDataService : IAssetDataService
    {
        private readonly IRepository repository;

        public AssetDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task<bool> UserHasRoleForAsset(int userId, UserAssetRole userAssetRole, int assetId)
        {
            int roleId = (int) userAssetRole;

            return repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.UserId == userId && x.AssetId == assetId && x.RoleId == roleId);
        }

        public Task<bool> UserHasRolesForAsset(int userId, UserAssetRole[] userAssetRoles, int assetId)
        {
            int[] roleIds = userAssetRoles.Select(x => (int) x).ToArray();

            return repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.UserId == userId && x.AssetId == assetId && roleIds.Contains(x.RoleId));
        }

        public Task<bool> UserHasRoleForDevice(int userId, UserAssetRole userAssetRole, int deviceId)
        {
            int roleId = (int) userAssetRole;

            return repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.UserId == userId && x.Asset.Devices.Any(y => y.Id == deviceId) && x.RoleId == roleId);
        }

        public Task<bool> HasActiveDeviceId(int assetId, string deviceId)
        {
            return repository.GetEntities<DeviceEntity>()
                .AnyAsync(x => x.AssetId == assetId && x.DeviceId == deviceId && x.IsActive);
        }
    }
}
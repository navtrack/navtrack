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

        public Task<bool> UserHasRole(int userId, int assetId, UserRole userRole)
        {
            int roleId = (int) userRole;

            return repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.UserId == userId && x.AssetId == assetId && x.RoleId == roleId);
        }

        public Task<bool> HasActiveDeviceId(int assetId, string deviceId)
        {
            return repository.GetEntities<DeviceEntity>()
                .AnyAsync(x => x.AssetId == assetId && x.DeviceId == deviceId && x.IsActive);
        }
    }
}
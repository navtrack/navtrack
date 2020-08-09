using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services
{
    [Service(typeof(IDeviceDataService))]
    public class DeviceDataService : IDeviceDataService
    {
        private readonly IRepository repository;

        public DeviceDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task<bool> DeviceIsAssigned(string deviceId, int protocolPort, int? excludeAssetId)
        {
            return repository.GetEntities<DeviceEntity>()
                .AnyAsync(x =>
                    x.DeviceId == deviceId && x.ProtocolPort == protocolPort && x.IsActive &&
                    (!excludeAssetId.HasValue || x.AssetId != excludeAssetId));
        }

        public Task<DeviceEntity> GetActiveDeviceByDeviceId(string deviceId)
        {
            return repository.GetEntities<DeviceEntity>()
                .FirstOrDefaultAsync(x => x.DeviceId == deviceId && x.IsActive);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;

namespace Navtrack.Listener.DataServices
{
    [Service(typeof(IAssetDataService))]
    public class AssetDataService : IAssetDataService
    {
        private readonly IRepository repository;

        public AssetDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task<AssetEntity> GetAssetByIMEI(string imei)
        {
            return repository.GetEntities<AssetEntity>()
                .Include(x => x.Device)
                .FirstOrDefaultAsync(x => x.Device.DeviceId == imei);
        }
    }
}
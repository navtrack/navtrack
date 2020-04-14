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

        public Task<Asset> GetAssetByIMEI(string imei)
        {
            return repository.GetEntities<Asset>()
                .Include(x => x.Device)
                .FirstOrDefaultAsync(x => x.Device.IMEI == imei);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Listener.DataServices;
using Location = Navtrack.Listener.Models.Location;

namespace Navtrack.Listener.Services
{
    [Service(typeof(ILocationService))]
    public class LocationService : ILocationService
    {
        private readonly ILocationDataService locationDataService;
        private readonly IAssetDataService assetDataService;
        private readonly IMapper mapper;

        public LocationService(ILocationDataService locationDataService, IMapper mapper,
            IAssetDataService assetDataService)
        {
            this.locationDataService = locationDataService;
            this.mapper = mapper;
            this.assetDataService = assetDataService;
        }

        public async Task Add(Location location)
        {
            Asset asset = await assetDataService.GetAssetByIMEI(location.Device.IMEI);

            if (asset != null)
            {
                DataAccess.Model.Location mapped =
                    mapper.Map<Location, Asset, DataAccess.Model.Location>(location, asset);

                await locationDataService.Add(mapped);
            }
        }

        public async Task AddRange(List<Location> locations)
        {
            if (locations.Any())
            {
                string imei = locations.First().Device.IMEI;

                Asset asset = await assetDataService.GetAssetByIMEI(imei);

                if (asset != null)
                {
                    List<DataAccess.Model.Location> mapped =
                        locations.Select(x => mapper.Map<Location, Asset, DataAccess.Model.Location>(x, asset))
                            .ToList();

                    await locationDataService.AddRange(mapped);
                }
            }
        }
    }
}
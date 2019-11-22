using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Common.DataServices;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Location = Navtrack.Common.Model.Location;

namespace Navtrack.Common.Services
{
    [Service(typeof(ILocationService))]
    public class LocationService : ILocationService
    {
        private readonly ILocationDataService locationDataService;
        private readonly IObjectDataService objectDataService;
        private readonly IMapper mapper;

        public LocationService(ILocationDataService locationDataService, IMapper mapper,
            IObjectDataService objectDataService)
        {
            this.locationDataService = locationDataService;
            this.mapper = mapper;
            this.objectDataService = objectDataService;
        }

        public async Task Add(Location location)
        {
            Asset asset = await objectDataService.GetObjectByIMEI(location.Device.IMEI);

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

                Asset asset = await objectDataService.GetObjectByIMEI(imei);

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
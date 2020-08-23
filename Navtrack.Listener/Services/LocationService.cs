using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Services;
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
        private readonly IDeviceDataService deviceDataService;
        private readonly IMapper mapper;

        public LocationService(ILocationDataService locationDataService, IMapper mapper,
            IDeviceDataService deviceDataService)
        {
            this.locationDataService = locationDataService;
            this.mapper = mapper;
            this.deviceDataService = deviceDataService;
        }

        public async Task Add(Location location)
        {
            DeviceEntity device = await deviceDataService.GetActiveDeviceByDeviceId(location.Device.IMEI);

            if (device != null)
            {
                LocationEntity mapped =
                    mapper.Map<Location, DeviceEntity, LocationEntity>(location, device);

                await locationDataService.Add(mapped);
            }
        }

        public async Task AddRange(List<Location> locations)
        {
            if (locations.Any())
            {
                string deviceId = locations.First().Device.IMEI;

                DeviceEntity device = await deviceDataService.GetActiveDeviceByDeviceId(deviceId);

                if (device != null)
                {
                    List<LocationEntity> mapped =
                        locations.Select(x => mapper.Map<Location, DeviceEntity, LocationEntity>(x, device))
                            .ToList();

                    await locationDataService.AddRange(mapped);
                }
            }
        }
    }
}
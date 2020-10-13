using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
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

        public async Task AddRange(List<Location> locations, int connectionMessageId)
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

                    foreach (LocationEntity locationEntity in mapped)
                    {
                        locationEntity.DeviceConnectionMessageId = connectionMessageId;
                    }

                    await locationDataService.AddRange(mapped);
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.Library.DI;
using Navtrack.Listener.Mappers;
using Location = Navtrack.Listener.Models.Location;

namespace Navtrack.Listener.Services;

[Service(typeof(ILocationService))]
public class LocationService : ILocationService
{
    private readonly ILocationDataService locationDataService;
    private readonly IDeviceDataService deviceDataService;

    public LocationService(ILocationDataService locationDataService, IDeviceDataService deviceDataService)
    {
        this.locationDataService = locationDataService;
        this.deviceDataService = deviceDataService;
    }

    public async Task AddRange(List<Location> locations, ObjectId connectionMessageId)
    {
        if (locations.Any())
        {
            string deviceId = locations.First().Device.IMEI;

            AssetDocument asset = await deviceDataService.GetActiveDeviceByDeviceId(deviceId);

            if (asset != null)
            {
                List<LocationDocument> mapped =
                    locations.Select(x => LocationDocumentMapper.Map(x, asset))
                        .ToList();

                foreach (LocationDocument locationEntity in mapped)
                {
                    locationEntity.DeviceConnectionMessageId = connectionMessageId;
                }

                await locationDataService.AddRange(mapped);
            }
        }
    }
}
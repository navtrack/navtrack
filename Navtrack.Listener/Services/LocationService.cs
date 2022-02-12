using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Services.Assets;
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
    private readonly IAssetDataService assetDataService;

    public LocationService(ILocationDataService locationDataService, IDeviceDataService deviceDataService,
        IAssetDataService assetDataService)
    {
        this.locationDataService = locationDataService;
        this.deviceDataService = deviceDataService;
        this.assetDataService = assetDataService;
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
                    locations.Select(x => LocationDocumentMapper.Map(x, asset, connectionMessageId))
                        .ToList();

                await locationDataService.AddRange(mapped);
                
                LocationDocument latestLocation = mapped.OrderByDescending(x => x.DateTime).First();

                if (asset.Location == null || latestLocation.DateTime > asset.Location.DateTime)
                {
                    await assetDataService.UpdateLocation(asset.Id, latestLocation);
                }
            }
        }
    }
}
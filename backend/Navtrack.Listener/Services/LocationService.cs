using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.DataAccess.Services.Locations;
using Navtrack.Listener.Mappers;
using Navtrack.Listener.Models;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Services;

[Service(typeof(ILocationService))]
public class LocationService(
    ILocationRepository repository,
    IDeviceRepository deviceRepository,
    IAssetRepository assetRepository)
    : ILocationService
{
    public async Task AddRange(List<Location> locations, ObjectId connectionMessageId)
    {
        if (locations.Any())
        {
            string deviceId = locations.First().Device.SerialNumber;

            AssetDocument asset = await deviceRepository.GetActiveDeviceByDeviceId(deviceId);

            if (asset != null)
            {
                List<LocationDocument> mapped =
                    locations.Select(x => LocationDocumentMapper.Map(x, asset, connectionMessageId))
                        .ToList();

                await repository.AddRange(mapped);
                
                LocationDocument latestLocation = mapped.OrderByDescending(x => x.DateTime).First();

                if (asset.Location == null || latestLocation.DateTime > asset.Location.DateTime)
                {
                    await assetRepository.UpdateLocation(asset.Id, latestLocation);
                }
            }
        }
    }
}
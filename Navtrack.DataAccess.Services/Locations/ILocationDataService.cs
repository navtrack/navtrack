using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.DataAccess.Services.Locations;

public interface ILocationDataService
{
    Task<List<LocationDocument>> GetLocations(string assetId, LocationFilter locationFilter);
    Task<List<LocationDocument>> GetLocations(string assetId, DateFilter locationFilter);
    Task<Dictionary<ObjectId, int>> GetLocationsCountByDeviceIds(IEnumerable<ObjectId> deviceIds);
    Task DeleteByAssetId(string assetId);
    Task AddRange(IEnumerable<LocationDocument> locations);
}
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Positions;

public interface IPositionRepository : IGenericRepository<PositionDocument>
{
    Task<GetPositionsResult> GetPositions(GetPositionsOptions options);
    Task<Dictionary<ObjectId, int>> GetLocationsCountByDeviceIds(IEnumerable<ObjectId> deviceIds);
    Task DeleteByAssetId(string assetId);
    Task<bool> DeviceHasLocations(string assetId, string deviceId);
}
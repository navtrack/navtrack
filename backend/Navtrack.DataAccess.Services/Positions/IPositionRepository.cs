using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Positions;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.DataAccess.Services.Positions;

public interface IPositionRepository : IGenericRepository<PositionGroupDocument>
{
    Task AddPositions(ObjectId positionGroupId, DateTime maxEndDate, IEnumerable<PositionElement> positions);
    Task<GetPositionsResult> GetPositions(string assetId, LocationFilter locationFilter, int page, int size);
    Task<List<PositionGroupDocument>> GetPositions(string assetId, DateFilter dateFilter);
    Task<Dictionary<ObjectId, int>> GetLocationsCountByDeviceIds(IEnumerable<ObjectId> deviceIds);
    Task DeleteByAssetId(string assetId);
    Task<bool> DeviceHasLocations(string assetId, string deviceId);
}
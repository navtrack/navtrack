using System;
using System.Linq;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Database.Model.Filters;

public class GetMessagesOptions
{
    public Guid AssetId { get; set; }
    public PositionFilterModel PositionFilter { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }
    public Func<IQueryable<DeviceMessageEntity>,IQueryable<DeviceMessageEntity>>? OrderFunc { get; set; }
}
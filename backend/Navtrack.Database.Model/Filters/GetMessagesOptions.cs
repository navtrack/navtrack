using System;

namespace Navtrack.Database.Model.Filters;

public class GetMessagesOptions
{
    public Guid AssetId { get; set; }
    public PositionFilterModel PositionFilter { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }
    // public SortDefinition<DeviceMessageEntity> OrderFunc { get; set; }
}
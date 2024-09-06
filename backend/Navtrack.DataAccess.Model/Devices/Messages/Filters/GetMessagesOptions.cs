using MongoDB.Driver;

namespace Navtrack.DataAccess.Model.Devices.Messages.Filters;

public class GetMessagesOptions
{
    public string AssetId { get; set; }
    public PositionFilter PositionFilter { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }
    public SortDefinition<DeviceMessageDocument>? OrderFunc { get; set; }
}
using MongoDB.Driver;

namespace Navtrack.DataAccess.Model.Positions;

public class GetMessagesOptions
{
    public string AssetId { get; set; }
    public PositionFilter PositionFilter { get; set; }
    public int? Page { get; set; }
    public int? Size { get; set; }
    public SortDefinition<MessageDocument>? OrderFunc { get; set; }
}
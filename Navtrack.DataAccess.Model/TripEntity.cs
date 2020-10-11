using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class TripEntity : CreatedEntityAudit, IEntity 
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int Number { get; set; }
        public AssetEntity Asset { get; set; }
        public LocationEntity StartLocation { get; set; }
        public int StartLocationId { get; set; }
        public LocationEntity EndLocation { get; set; }
        public int EndLocationId { get; set; }
        public int Distance { get; set; }
    }
}
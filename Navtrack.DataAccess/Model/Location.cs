using System;

namespace Navtrack.DataAccess.Model
{
    public class Location : EntityAudit, IEntity
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public decimal? Altitude { get; set; }
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public Asset Asset { get; set; }
        public int AssetId { get; set; }
        public short? Satellites { get; set; }
        public decimal? HDOP { get; set; }
        public bool? PositionStatus { get; set; }
        public short? GsmSignal { get; set; }
        public double? Odometer { get; set; }
    }
}
using System;

namespace Navtrack.DataAccess.Model
{
    public class Location : EntityAudit, IEntity
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public double Speed { get; set; }
        public float? Heading { get; set; }
        public double? Altitude { get; set; }
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public Asset Asset { get; set; }
        public int AssetId { get; set; }
        public short? Satellites { get; set; }
        public double? HDOP { get; set; }
        public string ProtocolData { get; set; }
        public bool PositionStatus { get; set; }
        public short? GsmSignal { get; set; }
        public double? Odometer { get; set; }
    }
}
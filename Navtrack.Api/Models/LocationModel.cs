using System;

namespace Navtrack.Api.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public double Speed { get; set; }
        public float? Heading { get; set; }
        public double? Altitude { get; set; }
        public short? Satellites { get; set; }
        public double? HDOP { get; set; }
        public bool PositionStatus { get; set; }
        public short? GsmSignal { get; set; }
        public double? Odometer { get; set; }
    }
}
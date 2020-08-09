using System;

namespace Navtrack.Api.Model.Locations
{
    public class LocationResponseModel
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public decimal? Altitude { get; set; }
        public short? Satellites { get; set; }
        public decimal? HDOP { get; set; }
        public bool? PositionStatus { get; set; }
        public short? GsmSignal { get; set; }
        public double? Odometer { get; set; }
    }
}
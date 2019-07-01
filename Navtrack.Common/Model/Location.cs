using System;

namespace Navtrack.Common.Model
{
    public class Location
    {
        public Device Device { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public int Speed { get; set; }
        public int Heading { get; set; }
        public int Altitude { get; set; }
        public short Satellites { get; set; }
        public double HDOP { get; set; }
    }
}
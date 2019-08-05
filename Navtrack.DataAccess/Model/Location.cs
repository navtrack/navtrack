using System;

namespace Navtrack.DataAccess.Model
{
    public class Location
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public int Speed { get; set; }
        public int Heading { get; set; }
        public int Altitude { get; set; }
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public Object Object { get; set; }
        public int ObjectId { get; set; }
        public short Satellites { get; set; }
        public double HDOP { get; set; }
        public string ProtocolData { get; set; }
    }
}
using System;
using System.Text.Json;

namespace Navtrack.Listener.Models
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
        public string ProtocolData { get; set; }
    }

    public class Location<T> : Location
    {
        public T Data
        {
            get => JsonSerializer.Deserialize<T>(ProtocolData);
            set => ProtocolData = JsonSerializer.Serialize(value);
        }
    }
}
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Protocols.Teltonika
{
    public class LocationHolder
    {
        public Location Location { get; set; }
        public TeltonikaData ProtocolData { get; set; }
    }
}
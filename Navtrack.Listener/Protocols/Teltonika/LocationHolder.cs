using Navtrack.Common.Model;

namespace Navtrack.Listener.Protocols.Teltonika
{
    public class LocationHolder
    {
        public Location Location { get; set; }
        public TeltonikaData ProtocolData { get; set; }
    }
}
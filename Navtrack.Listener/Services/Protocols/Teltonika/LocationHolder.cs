using Navtrack.Common.Model;

namespace Navtrack.Listener.Services.Protocols.Teltonika
{
    public class LocationHolder
    {
        public Location Location { get; set; }
        public TeltonikaData ProtocolData { get; set; }
    }
}
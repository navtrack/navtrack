using System.Collections.Generic;

namespace Navtrack.Listener.Services.Protocols.Teltonika
{
    public interface ITeltonikaLocationParser
    {
        List<LocationHolder> Convert(byte[] input, string imei);
    }
}
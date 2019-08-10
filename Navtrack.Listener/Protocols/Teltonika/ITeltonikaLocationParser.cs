using System.Collections.Generic;

namespace Navtrack.Listener.Protocols.Teltonika
{
    public interface ITeltonikaLocationParser
    {
        List<LocationHolder> Convert(byte[] input, string imei);
        string GetIMEI(byte[] bytes);
    }
}
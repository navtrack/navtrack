using System.Collections.Generic;
using Navtrack.Common.Model;

namespace Navtrack.Listener.Services.Protocols.Teltonika
{
    public interface ITeltonikaLocationParser
    {
        IEnumerable<Location> Convert(byte[] input, string imei);
    }
}
using System.Collections.Generic;
using Navtrack.DeviceData.Model;

namespace Navtrack.DeviceData.Services
{
    public interface IProtocolDataService
    {
        IEnumerable<Protocol> GetProtocols();
    }
}
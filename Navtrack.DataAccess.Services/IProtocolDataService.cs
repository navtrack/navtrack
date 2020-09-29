using System.Collections.Generic;
using Navtrack.DeviceData.Model;

namespace Navtrack.DataAccess.Services
{
    public interface IProtocolDataService
    {
        IEnumerable<Protocol> GetProtocols();
    }
}
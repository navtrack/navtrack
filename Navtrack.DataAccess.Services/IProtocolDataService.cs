using System.Collections.Generic;
using Navtrack.DataAccess.Model.Custom;

namespace Navtrack.DataAccess.Services
{
    public interface IProtocolDataService
    {
        IEnumerable<Protocol> GetProtocols();
    }
}
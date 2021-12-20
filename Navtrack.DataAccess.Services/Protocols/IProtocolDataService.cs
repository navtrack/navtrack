using System.Collections.Generic;
using Navtrack.DataAccess.Model.Protocols;

namespace Navtrack.DataAccess.Services.Protocols;

public interface IProtocolDataService
{
    IEnumerable<Protocol> GetProtocols();
}
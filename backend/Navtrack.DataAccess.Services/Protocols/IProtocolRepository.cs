using System.Collections.Generic;
using Navtrack.DataAccess.Model.Protocols;

namespace Navtrack.DataAccess.Services.Protocols;

public interface IProtocolRepository
{
    IEnumerable<Protocol> GetProtocols();
}
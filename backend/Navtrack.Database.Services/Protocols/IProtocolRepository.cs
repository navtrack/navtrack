using System.Collections.Generic;
using Navtrack.Database.Model.Protocols;

namespace Navtrack.Database.Services.Protocols;

public interface IProtocolRepository
{
    IEnumerable<Protocol> GetProtocols();
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Database.Model.Protocols;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Protocols;

[Service(typeof(IProtocolRepository), ServiceLifetime.Singleton)]
public class ProtocolRepository(IDeviceTypeRepository typeRepository) : IProtocolRepository
{
    private Protocol[]? protocols;

    public IEnumerable<Protocol> GetProtocols()
    {
        return protocols ??= typeRepository.GetDeviceTypes()
            .GroupBy(x => x.Protocol)
            .Select(x => x.Key)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Port)
            .ToArray();
    }
}
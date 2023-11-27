using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DataAccess.Model.Protocols;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.DataAccess.Services.Protocols;

[Service(typeof(IProtocolRepository), ServiceLifetime.Singleton)]
public class ProtocolRepository : IProtocolRepository
{
    private readonly IDeviceTypeRepository deviceTypeRepository;
    private Protocol[]? protocols;

    public ProtocolRepository(IDeviceTypeRepository deviceTypeRepository)
    {
        this.deviceTypeRepository = deviceTypeRepository;
    }

    public IEnumerable<Protocol> GetProtocols()
    {
        return protocols ??= deviceTypeRepository.GetDeviceTypes()
            .GroupBy(x => x.Protocol)
            .Select(x => x.Key)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Port)
            .ToArray();
    }
}
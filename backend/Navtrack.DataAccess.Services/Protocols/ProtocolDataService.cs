using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DataAccess.Model.Protocols;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Protocols;

[Service(typeof(IProtocolDataService), ServiceLifetime.Singleton)]
public class ProtocolDataService : IProtocolDataService
{
    private readonly IDeviceTypeDataService deviceTypeDataService;
    private Protocol[]? protocols;

    public ProtocolDataService(IDeviceTypeDataService deviceTypeDataService)
    {
        this.deviceTypeDataService = deviceTypeDataService;
    }

    public IEnumerable<Protocol> GetProtocols()
    {
        return protocols ??= deviceTypeDataService.GetDeviceTypes()
            .GroupBy(x => x.Protocol)
            .Select(x => x.Key)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Port)
            .ToArray();
    }
}
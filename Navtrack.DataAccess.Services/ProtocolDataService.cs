using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services
{
    [Service(typeof(IProtocolDataService), ServiceLifetime.Singleton)]
    public class ProtocolDataService : IProtocolDataService
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private Protocol[] protocols;

        public ProtocolDataService(IDeviceTypeDataService deviceTypeDataService)
        {
            this.deviceTypeDataService = deviceTypeDataService;
        }

        public IEnumerable<Protocol> GetProtocols()
        {
            if (protocols == null)
            {
                protocols = deviceTypeDataService.GetDeviceTypes()
                    .GroupBy(x => x.Protocol)
                    .Select(x => x.Key)
                    .OrderBy(x => x.Name)
                    .ThenBy(x => x.Port)
                    .ToArray();
            }

            return protocols;
        }
    }
}
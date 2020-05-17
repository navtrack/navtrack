using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;

namespace Navtrack.DeviceData.Services
{
    [Service(typeof(IProtocolDataService), ServiceLifetime.Singleton)]
    public class ProtocolDataService : IProtocolDataService
    {
        private readonly IDeviceModelDataService deviceModelDataService;
        private Protocol[] protocols;

        public ProtocolDataService(IDeviceModelDataService deviceModelDataService)
        {
            this.deviceModelDataService = deviceModelDataService;
        }

        public Protocol[] GetProtocols()
        {
            if (protocols == null)
            {
                protocols = deviceModelDataService.GetDeviceModels()
                    .GroupBy(x => new {x.Protocol, x.Port})
                    .Select(x => new Protocol
                    {
                        Name = x.Key.Protocol,
                        Port = x.Key.Port
                    })
                    .OrderBy(x => x.Name)
                    .ThenBy(x => x.Port)
                    .ToArray();
            }

            return protocols;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Models;
using Navtrack.DeviceData.Model;
using Navtrack.DeviceData.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services
{
    [Service(typeof(IProtocolService))]
    public class ProtocolService : IProtocolService
    {
        private readonly IProtocolDataService protocolDataService;
        private readonly IMapper mapper;

        public ProtocolService(IProtocolDataService protocolDataService, IMapper mapper)
        {
            this.protocolDataService = protocolDataService;
            this.mapper = mapper;
        }

        public List<ProtocolModel> GetProtocols()
        {
            return protocolDataService.GetProtocols()
                .Select(mapper.Map<Protocol, ProtocolModel>)
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}
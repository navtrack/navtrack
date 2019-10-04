using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;

namespace Navtrack.Web.Services
{
    [Service(typeof(IDeviceService))]
    public class DeviceService : IDeviceService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public DeviceService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<Device>> GetDevices()
        {
            List<DataAccess.Model.Device> devices =
                await repository.GetEntities<Navtrack.DataAccess.Model.Device>().ToListAsync();

            List<Device> mapped = devices.Select(x => mapper.Map<Navtrack.DataAccess.Model.Device, Device>(x)).ToList();

            return mapped;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;

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

        public async Task<List<DeviceModel>> Get()
        {
            List<DataAccess.Model.Device> devices =
                await repository.GetEntities<Navtrack.DataAccess.Model.Device>().ToListAsync();

            List<DeviceModel> mapped = devices.Select(mapper.Map<Navtrack.DataAccess.Model.Device, DeviceModel>).ToList();

            return mapped;
        }

        public async Task Add(DeviceModel deviceModel)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            unitOfWork.Add(new DataAccess.Model.Device
            {
                IMEI = deviceModel.IMEI
            });

            await unitOfWork.SaveChanges();
        }

        public Task<bool> IsValidNewDevice(DeviceModel deviceModel)
        {
            return repository.GetEntities<DataAccess.Model.Device>().AllAsync(x => x.IMEI != deviceModel.IMEI);
        }
    }
}
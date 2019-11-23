using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;
using DeviceModel = Navtrack.Web.Model.Models.DeviceModel;

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

        public async Task<List<DeviceModel>> GetAll()
        {
            List<Device> devices =
                await repository.GetEntities<Device>().ToListAsync();

            List<DeviceModel> mapped = devices.Select(mapper.Map<Device, DeviceModel>)
                .ToList();

            return mapped;
        }

        public async Task Add(DeviceModel deviceModel)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            Device mapped = mapper.Map<DeviceModel, Device>(deviceModel);

            unitOfWork.Add(mapped);

            await unitOfWork.SaveChanges();
        }

        public async Task<DeviceModel> Get(int id)
        {
            Device device = await 
                repository.GetEntities<Device>().FirstOrDefaultAsync(x => x.Id == id);

            return device != null
                ? mapper.Map<Device, DeviceModel>(device)
                : null;
        }

        public IEnumerable<ProtocolModel> GetProtocols()
        {
            return Enum.GetValues(typeof(Protocol)).Cast<Protocol>()
                .Select(mapper.Map<Protocol, ProtocolModel>)
                .OrderBy(x => x.Name)
                .ToList();
        }

        public async Task Update(DeviceModel model)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            Device mapped = mapper.Map<DeviceModel, Device>(model);

            unitOfWork.Update(mapped);

            await unitOfWork.SaveChanges();
        }

        public Task<bool> IMEIAlreadyExists(string imei)
        {
            return repository.GetEntities<Device>().AnyAsync(x => x.IMEI == imei);
        }

        public async Task<List<DeviceTypeModel>> GetTypes()
        {
            List<DeviceType> devices =
                await repository.GetEntities<DeviceType>()
                    .OrderBy(x => x.Brand)
                    .ThenBy(x => x.Model)
                    .ToListAsync();

            List<DeviceTypeModel> mapped = devices.Select(mapper.Map<DeviceType, DeviceTypeModel>)
                .ToList();

            return mapped;
        }
    }
}
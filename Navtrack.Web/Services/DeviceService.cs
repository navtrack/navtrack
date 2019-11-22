using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Navtrack.Common.Devices;
using Navtrack.DataAccess.Model;
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

        public async Task<List<DeviceModel>> Get()
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

            unitOfWork.Add(new Device
            {
                IMEI = deviceModel.IMEI
            });

            await unitOfWork.SaveChanges();
        }

        public Task<bool> IsValidNewDevice(DeviceModel model)
        {
            return repository.GetEntities<Device>().AllAsync(x => x.IMEI != model.IMEI);
        }

        public async Task<DeviceModel> Get(int id)
        {
            Device device = await 
                repository.GetEntities<Device>().FirstOrDefaultAsync(x => x.Id == id);

            return device != null
                ? mapper.Map<Device, DeviceModel>(device)
                : null;
        }

        public List<ProtocolModel> GetProtocols()
        {
            return Enum.GetValues(typeof(Protocol)).Cast<Protocol>().Select(mapper.Map<Protocol, ProtocolModel>)
                .ToList();
        }

        public async Task Update(DeviceModel model)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            Device mapped = mapper.Map<DeviceModel, Device>(model);

            unitOfWork.Update(mapped);

            await unitOfWork.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
                await repository.GetEntities<Device>()
                    .Include(x => x.DeviceType)
                    .ToListAsync();

            List<DeviceModel> mapped = devices.Select(mapper.Map<Device, DeviceModel>)
                .ToList();

            return mapped;
        }

        public async Task Add(DeviceModel device)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();

            Device mapped = mapper.Map<DeviceModel, Device>(device);

            unitOfWork.Add(mapped);

            await unitOfWork.SaveChanges();
        }

        public async Task<DeviceModel> Get(int id)
        {
            Device device = await 
                repository.GetEntities<Device>()
                    .Include(x => x.DeviceType)
                    .FirstOrDefaultAsync(x => x.Id == id);

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

        public async Task Update(DeviceModel device)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            
            Device mapped = mapper.Map<DeviceModel, Device>(device);

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

        public async Task ValidateModel(DeviceModel device, ModelStateDictionary modelState)
        {
            if (await repository.GetEntities<Device>().AnyAsync(x => x.IMEI == device.IMEI && x.Id != device.Id))
            {
                modelState.AddModelError(nameof(DeviceModel.IMEI), "IMEI already exists in the database.");
            }

            if (await repository.GetEntities<DeviceType>().AllAsync(x => x.Id != device.DeviceTypeId))
            {
                modelState.AddModelError(nameof(DeviceModel.IMEI), "No such device type.");
            }
        }

        public async Task<List<DeviceModel>> GetAllAvailableIncluding(int? id)
        {
            List<Device> devices =
                await repository.GetEntities<Device>()
                    .Include(x => x.DeviceType)
                    .Where(x => x.Asset == null || (id.HasValue && x.Id == id))
                    .ToListAsync();

            List<DeviceModel> mapped = devices.Select(mapper.Map<Device, DeviceModel>)
                .ToList();

            return mapped;
        }
    }
}
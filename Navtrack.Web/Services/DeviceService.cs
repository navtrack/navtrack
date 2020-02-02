using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Extensions;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    [Service(typeof(IDeviceService))]
    [Service(typeof(IGenericService<Device, DeviceModel>))]
    public class DeviceService : GenericService<Device, DeviceModel>, IDeviceService
    {
        public DeviceService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(repository, mapper, httpContextAccessor)
        {
        }

        protected override IQueryable<Device> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.DeviceType)
                .Where(x => x.Users.Any(y => y.UserId == httpContextAccessor.HttpContext.User.GetId()));
        }

        public IEnumerable<ProtocolModel> GetProtocols()
        {
            return Enum.GetValues(typeof(Protocol)).Cast<Protocol>()
                .Select(mapper.Map<Protocol, ProtocolModel>)
                .OrderBy(x => x.Name)
                .ToList();
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

        protected override async Task ValidateSave(DeviceModel device, ValidationResult validationResult)
        {
            if (string.IsNullOrEmpty(device.IMEI) || device.IMEI.Length != 15)
            {
                validationResult.AddError(nameof(DeviceModel.IMEI), "The IMEI must be 15 characters.");
            }
            else if (!device.IMEI.All(char.IsDigit))
            {
                validationResult.AddError(nameof(DeviceModel.IMEI), "The IMEI must be only digits.");
            }
            else if (await repository.GetEntities<Device>().AnyAsync(x => x.IMEI == device.IMEI && x.Id != device.Id))
            {
                validationResult.AddError(nameof(DeviceModel.IMEI), "The IMEI already exists in the database.");
            }
            if (await repository.GetEntities<DeviceType>().AllAsync(x => x.Id != device.DeviceTypeId))
            {
                validationResult.AddError(nameof(DeviceModel.IMEI), "No such device type.");
            }
        }

        public async Task<List<DeviceModel>> GetAllAvailableForAsset(int? assetId)
        {
            List<Device> devices =
                await repository.GetEntities<Device>()
                    .Include(x => x.DeviceType)
                    .Where(x => (x.Asset == null || assetId.HasValue && x.Asset.Id == assetId) &&
                                x.Users.Any(y => y.UserId == httpContextAccessor.HttpContext.User.GetId()))
                    .ToListAsync();

            List<DeviceModel> mapped = devices.Select(MapToModel).ToList();

            return mapped;
        }

        protected override async Task ValidateDelete(int id, ValidationResult validationResult)
        {
            if (await repository.GetEntities<Device>().AnyAsync(x => x.Id == id && x.Asset != null))
            {
                validationResult.Title = "Device is assigned to an asset.";
            }
        }

        protected override Task BeforeAdd(IUnitOfWork unitOfWork, DeviceModel model, Device entity)
        {
            entity.Users.Add(new UserDevice
            {
                UserId = httpContextAccessor.HttpContext.User.GetId(),
                RoleId = (int) Role.Owner
            });

            return Task.CompletedTask;
        }

        protected override async Task<IUserRelation> GetUserRelation(int id)
        {
            UserDevice userDevice = await repository.GetEntities<UserDevice>()
                .FirstOrDefaultAsync(x => x.DeviceId == id);

            return userDevice;
        }
    }
}
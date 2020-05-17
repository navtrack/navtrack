using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Generic;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Repository;
using Navtrack.DeviceData.Services;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services
{
    [Service(typeof(IDeviceService))]
    [Service(typeof(IGenericService<DeviceEntity, DeviceModel>))]
    public class DeviceService : GenericService<DeviceEntity, DeviceModel>, IDeviceService
    {
        private readonly IDeviceModelDataService deviceModelDataService;

        public DeviceService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IDeviceModelDataService deviceModelDataService) : base(repository, mapper, httpContextAccessor)
        {
            this.deviceModelDataService = deviceModelDataService;
        }

        protected override IQueryable<DeviceEntity> GetQueryable()
        {
            return base.GetQueryable()
                .Where(x => x.Users.Any(y => y.UserId == httpContextAccessor.HttpContext.User.GetId()));
        }

        protected override async Task ValidateSave(DeviceModel device, ValidationResult validationResult)
        {
            if (string.IsNullOrEmpty(device.DeviceId))
            {
                validationResult.AddError(nameof(DeviceModel.DeviceId), "The Device ID is required.");
            }
            else if (await repository.GetEntities<DeviceEntity>()
                .AnyAsync(x => x.DeviceId == device.DeviceId && x.Id != device.Id))
            {
                validationResult.AddError(nameof(DeviceModel.DeviceId),
                    "The Device ID already exists in the database.");
            }
            if (deviceModelDataService.GetById(device.DeviceModelId) == null)
            {
                validationResult.AddError(nameof(device.DeviceModelId), "No such device model.");
            }
        }

        public async Task<List<DeviceModel>> GetAllAvailableForAsset(int? assetId)
        {
            List<DeviceEntity> devices =
                await repository.GetEntities<DeviceEntity>()
                    .Where(x => (x.Asset == null || assetId.HasValue && x.Asset.Id == assetId) &&
                                x.Users.Any(y => y.UserId == httpContextAccessor.HttpContext.User.GetId()))
                    .ToListAsync();

            List<DeviceModel> mapped = devices.Select(MapToModel).ToList();

            return mapped;
        }

        protected override async Task ValidateDelete(int id, ValidationResult validationResult)
        {
            if (await repository.GetEntities<DeviceEntity>().AnyAsync(x => x.Id == id && x.Asset != null))
            {
                validationResult.Title = "Device is assigned to an asset.";
            }
        }

        protected override Task BeforeAdd(IUnitOfWork unitOfWork, DeviceModel model, DeviceEntity entity)
        {
            entity.Users.Add(new UserDeviceEntity
            {
                UserId = httpContextAccessor.HttpContext.User.GetId(),
                RoleId = (int) EntityRole.Owner
            });

            return Task.CompletedTask;
        }

        protected override async Task<IUserRelation> GetUserRelation(int id)
        {
            UserDeviceEntity userDevice = await repository.GetEntities<UserDeviceEntity>()
                .FirstOrDefaultAsync(x => x.DeviceId == id);

            return userDevice;
        }
    }
}
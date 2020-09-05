using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Assets.Requests;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.DeviceData.Model;
using Navtrack.DeviceData.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IRequestHandler<ChangeDeviceRequest>))]
    public class ChangeDeviceRequestHandler : BaseRequestHandler<ChangeDeviceRequest>
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;
        private readonly IDeviceService deviceService;

        public ChangeDeviceRequestHandler(IDeviceTypeDataService deviceTypeDataService,
            IRepository repository, IAssetDataService assetDataService, IDeviceService deviceService)
        {
            this.deviceTypeDataService = deviceTypeDataService;
            this.repository = repository;
            this.assetDataService = assetDataService;
            this.deviceService = deviceService;
        }

        public override async Task Authorize(ChangeDeviceRequest request)
        {
            if (!await assetDataService.UserHasRoleForAsset(request.UserId, UserAssetRole.Owner, request.AssetId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task Validate(ChangeDeviceRequest request)
        {
            if (!deviceTypeDataService.Exists(request.Body.DeviceTypeId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceTypeId),
                    "The device type does not exist.");
            }
            else if (string.IsNullOrEmpty(request.Body.DeviceId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceId),
                    "The device ID is required.");
            }
            else if (await deviceService.DeviceIsUsed(request.Body.DeviceId, request.Body.DeviceTypeId,
                request.AssetId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceId),
                    "The device ID is already assigned to another asset.");
            }
            else if (await assetDataService.HasActiveDeviceId(request.AssetId, request.Body.DeviceId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceId),
                    "The device ID is already assigned to this asset.");
            }
        }

        public override async Task Handle(ChangeDeviceRequest request)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            await unitOfWork.BeginTransaction();

            AssetEntity asset = await GetAsset(unitOfWork, request.AssetId);
            DeviceEntity activeDevice = asset.Devices.FirstOrDefault(x => x.IsActive);

            if (activeDevice != null)
            {
                activeDevice.IsActive = false;
                unitOfWork.Update(activeDevice);
            }
            
            DeviceEntity existingDevice = asset.Devices.FirstOrDefault(x =>
                x.DeviceId == request.Body.DeviceId && x.DeviceTypeId == request.Body.DeviceTypeId);

            if (existingDevice != null)
            {
                existingDevice.IsActive = true;
                unitOfWork.Update(existingDevice);
            }
            else
            {
                DeviceEntity deviceWithNoLocations = await GetAssetDeviceWithNoLocations(unitOfWork, request.AssetId);

                if (deviceWithNoLocations != null)
                {
                    deviceWithNoLocations.DeviceTypeId = request.Body.DeviceTypeId;
                    deviceWithNoLocations.DeviceId = request.Body.DeviceId;
                    deviceWithNoLocations.IsActive = true;
                    unitOfWork.Update(deviceWithNoLocations);
                    
                }
                else
                {
                    AddDevice(unitOfWork, request);
                }
            }
            
            await unitOfWork.SaveChanges();
            await unitOfWork.CommitTransaction();
        }

        private void AddDevice(IUnitOfWork unitOfWork, ChangeDeviceRequest request)
        {
            DeviceType deviceType = deviceTypeDataService.GetById(request.Body.DeviceTypeId);

            DeviceEntity deviceEntity = new DeviceEntity
            {
                DeviceId = request.Body.DeviceId,
                DeviceTypeId = request.Body.DeviceTypeId,
                AssetId = request.AssetId,
                ProtocolPort = deviceType.Protocol.Port,
                IsActive = true
            };

            unitOfWork.Add(deviceEntity);
        }

        private static Task<DeviceEntity> GetAssetDeviceWithNoLocations(IUnitOfWork unitOfWork, int assetId)
        {
            return unitOfWork.GetEntities<DeviceEntity>()
                .FirstOrDefaultAsync(x => x.AssetId == assetId && !x.Locations.Any());
        }

        private static Task<AssetEntity> GetAsset(IUnitOfWork unitOfWork, int assetId)
        {
            return unitOfWork.GetEntities<AssetEntity>()
                .Include(x => x.Devices)
                .FirstAsync(x => x.Id == assetId);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.Api.Services.Devices;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DataAccess.Services;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<ChangeDeviceCommand>))]
    public class ChangeDeviceCommandHandler : BaseCommandHandler<ChangeDeviceCommand>
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private readonly IRepository repository;
        private readonly IAssetDataService assetDataService;
        private readonly IDeviceService deviceService;

        public ChangeDeviceCommandHandler(IDeviceTypeDataService deviceTypeDataService,
            IRepository repository, IAssetDataService assetDataService, IDeviceService deviceService)
        {
            this.deviceTypeDataService = deviceTypeDataService;
            this.repository = repository;
            this.assetDataService = assetDataService;
            this.deviceService = deviceService;
        }

        public override async Task Authorize(ChangeDeviceCommand command)
        {
            if (!await assetDataService.UserHasRoleForAsset(command.UserId, UserAssetRole.Owner, command.AssetId))
            {
                ApiResponse.IsUnauthorised();
            }
        }

        public override async Task Validate(ChangeDeviceCommand command)
        {
            if (!deviceTypeDataService.Exists(command.Model.DeviceTypeId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceTypeId),
                    "The device type does not exist.");
            }
            else if (string.IsNullOrEmpty(command.Model.DeviceId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceId),
                    "The device ID is required.");
            }
            else if (await deviceService.DeviceIsUsed(command.Model.DeviceId, command.Model.DeviceTypeId,
                command.AssetId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceId),
                    "The device ID is already assigned to another asset.");
            }
            else if (await assetDataService.HasActiveDeviceId(command.AssetId, command.Model.DeviceId))
            {
                ApiResponse.AddError(nameof(ChangeDeviceRequestModel.DeviceId),
                    "The device ID is already assigned to this asset.");
            }
        }

        public override async Task Handle(ChangeDeviceCommand command)
        {
            using IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            await unitOfWork.BeginTransaction();

            AssetEntity asset = await GetAsset(unitOfWork, command.AssetId);
            DeviceEntity activeDevice = asset.Devices.FirstOrDefault(x => x.IsActive);

            if (activeDevice != null)
            {
                activeDevice.IsActive = false;
                unitOfWork.Update(activeDevice);
            }
            
            DeviceEntity existingDevice = asset.Devices.FirstOrDefault(x =>
                x.DeviceId == command.Model.DeviceId && x.DeviceTypeId == command.Model.DeviceTypeId);

            if (existingDevice != null)
            {
                existingDevice.IsActive = true;
                unitOfWork.Update(existingDevice);
            }
            else
            {
                DeviceEntity deviceWithNoLocations = await GetAssetDeviceWithNoLocations(unitOfWork, command.AssetId);

                if (deviceWithNoLocations != null)
                {
                    deviceWithNoLocations.DeviceTypeId = command.Model.DeviceTypeId;
                    deviceWithNoLocations.DeviceId = command.Model.DeviceId;
                    deviceWithNoLocations.IsActive = true;
                    unitOfWork.Update(deviceWithNoLocations);
                    
                }
                else
                {
                    AddDevice(unitOfWork, command);
                }
            }
            
            await unitOfWork.SaveChanges();
            await unitOfWork.CommitTransaction();
        }

        private void AddDevice(IUnitOfWork unitOfWork, ChangeDeviceCommand command)
        {
            DeviceType deviceType = deviceTypeDataService.GetById(command.Model.DeviceTypeId);

            DeviceEntity deviceEntity = new DeviceEntity
            {
                DeviceId = command.Model.DeviceId,
                DeviceTypeId = command.Model.DeviceTypeId,
                AssetId = command.AssetId,
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
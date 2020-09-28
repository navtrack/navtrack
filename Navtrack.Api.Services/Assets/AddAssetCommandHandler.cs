using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.DataAccess.Repository;
using Navtrack.DeviceData.Model;
using Navtrack.DeviceData.Services;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<AddAssetCommand, AddAssetModel>))]
    public class AddAssetCommandHandler : BaseCommandHandler<AddAssetCommand, AddAssetModel>
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private readonly IRepository repository;
        private readonly IDeviceService deviceService;

        public AddAssetCommandHandler(IDeviceTypeDataService deviceTypeDataService,
            IRepository repository, IDeviceService deviceService)
        {
            this.deviceTypeDataService = deviceTypeDataService;
            this.repository = repository;
            this.deviceService = deviceService;
        }

        public override async Task Validate(AddAssetCommand command)
        {
            if (!deviceTypeDataService.Exists(command.Model.DeviceTypeId))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.DeviceTypeId), "The device type is not valid.");

                return;
            }

            if (string.IsNullOrEmpty(command.Model.Name))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name), "The name is required.");
            }
            else if (await repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.UserId == command.UserId &&
                EF.Functions.Like(x.Asset.Name.ToLower(), $"{command.Model.Name.ToLower()}")))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name),
                    "You already have an asset with the same name.");
            }

            if (string.IsNullOrEmpty(command.Model.DeviceId))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.DeviceId),
                    "The device id is required.");
            }
            else if (await deviceService.DeviceIsUsed(command.Model.DeviceId, command.Model.DeviceTypeId))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.DeviceId),
                    "The device id is already used by another device on the same protocol.");
            }
        }

        public override async Task<AddAssetModel> Handle(AddAssetCommand command)
        {
            DeviceType deviceType = deviceTypeDataService.GetById(command.Model.DeviceTypeId);

            AssetEntity asset = await AddAsset(command, deviceType);

            return new AddAssetModel
            {
                Id = asset.Id
            };
        }

        private async Task<AssetEntity> AddAsset(AddAssetCommand command, DeviceType deviceType)
        {
            IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            await unitOfWork.BeginTransaction();

            AssetEntity asset = new AssetEntity
            {
                Name = command.Model.Name
            };

            UserAssetEntity userAssetEntity = new UserAssetEntity
            {
                UserId = command.UserId,
                Asset = asset,
                RoleId = (int) UserAssetRole.Owner
            };

            DeviceEntity device = new DeviceEntity
            {
                DeviceId = command.Model.DeviceId,
                DeviceTypeId = command.Model.DeviceTypeId,
                Asset = asset,
                ProtocolPort = deviceType.Protocol.Port,
                IsActive = true
            };

            unitOfWork.Add(asset);
            unitOfWork.Add(userAssetEntity);
            unitOfWork.Add(device);

            await unitOfWork.SaveChanges();
            await unitOfWork.CommitTransaction();
            
            return asset;
        }
    }
}
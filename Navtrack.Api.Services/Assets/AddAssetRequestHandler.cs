using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Assets.Requests;
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
    [Service(typeof(IRequestHandler<AddAssetRequest, AddAssetResponseModel>))]
    public class AddAssetRequestHandler : BaseRequestHandler<AddAssetRequest, AddAssetResponseModel>
    {
        private readonly IDeviceTypeDataService deviceTypeDataService;
        private readonly IRepository repository;
        private readonly IDeviceService deviceService;

        public AddAssetRequestHandler(IDeviceTypeDataService deviceTypeDataService,
            IRepository repository, IDeviceService deviceService)
        {
            this.deviceTypeDataService = deviceTypeDataService;
            this.repository = repository;
            this.deviceService = deviceService;
        }

        public override async Task Validate(AddAssetRequest request)
        {
            if (!deviceTypeDataService.Exists(request.Body.DeviceTypeId))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.DeviceTypeId), "The device type is not valid.");

                return;
            }

            if (string.IsNullOrEmpty(request.Body.Name))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name), "The name is required.");
            }
            else if (await repository.GetEntities<UserAssetEntity>().AnyAsync(x =>
                x.UserId == request.UserId &&
                EF.Functions.Like(x.Asset.Name.ToLower(), $"{request.Body.Name.ToLower()}")))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.Name),
                    "You already have an asset with the same name.");
            }

            if (string.IsNullOrEmpty(request.Body.DeviceId))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.DeviceId),
                    "The device id is required.");
            }
            else if (await deviceService.DeviceIsUsed(request.Body.DeviceId, request.Body.DeviceTypeId))
            {
                ApiResponse.AddError(nameof(AddAssetRequestModel.DeviceId),
                    "The device id is already used by another device on the same protocol.");
            }
        }

        public override async Task<AddAssetResponseModel> Handle(AddAssetRequest request)
        {
            DeviceType deviceType = deviceTypeDataService.GetById(request.Body.DeviceTypeId);

            AssetEntity asset = await AddAsset(request, deviceType);

            return new AddAssetResponseModel
            {
                Id = asset.Id
            };
        }

        private async Task<AssetEntity> AddAsset(AddAssetRequest request, DeviceType deviceType)
        {
            IUnitOfWork unitOfWork = repository.CreateUnitOfWork();
            await unitOfWork.BeginTransaction();

            AssetEntity asset = new AssetEntity
            {
                Name = request.Body.Name
            };

            UserAssetEntity userAssetEntity = new UserAssetEntity
            {
                UserId = request.UserId,
                Asset = asset,
                RoleId = (int) UserAssetRole.Owner
            };

            DeviceEntity device = new DeviceEntity
            {
                DeviceId = request.Body.DeviceId,
                DeviceTypeId = request.Body.DeviceTypeId,
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Assets.Requests;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.RequestHandlers;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IRequestHandler<GetAssetByIdRequest, AssetResponseModel>))]
    public class GetAssetByIdRequestHandler : BaseRequestHandler<GetAssetByIdRequest, AssetResponseModel>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetAssetByIdRequestHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public override async Task<AssetResponseModel> Handle(GetAssetByIdRequest request)
        {
            AssetEntity entity = await repository.GetEntities<AssetEntity>()
                .Include(x => x.Devices)
                .FirstOrDefaultAsync(x => x.Id == request.AssetId &&
                                          x.Users.Any(y => y.UserId == request.UserId));

            if (entity != null)
            {
                AssetResponseModel responseModel = mapper.Map<AssetEntity, AssetResponseModel>(entity);

                responseModel.Devices = await GetDevices(entity);

                return responseModel;
            }

            return null;
        }

        private async Task<List<DeviceModel>> GetDevices(AssetEntity entity)
        {
            var locationCount = await repository.GetEntities<LocationEntity>().Where(x => x.AssetId == entity.Id)
                .GroupBy(x => x.DeviceId)
                .Select(x => new {DeviceId = x.Key, LocationsCount = x.Count()})
                .ToListAsync();


            return entity.Devices.Select(x =>
            {
                DeviceModel deviceModel = mapper.Map<DeviceEntity, DeviceModel>(x);
                deviceModel.LocationsCount = (locationCount.FirstOrDefault(y => y.DeviceId == x.Id)?.LocationsCount)
                    .GetValueOrDefault();

                return deviceModel;
            }).ToList();
        }
    }
}
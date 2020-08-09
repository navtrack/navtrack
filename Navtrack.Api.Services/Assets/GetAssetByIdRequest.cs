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
                .FirstOrDefaultAsync(x => x.Id == request.AssetId &&
                                          x.Users.Any(y => y.UserId == request.UserId));

            if (entity != null)
            {
                List<DeviceEntity> devices = await repository.GetEntities<DeviceEntity>()
                    .Where(x => x.AssetId == request.AssetId)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                var locations = await repository.GetEntities<LocationEntity>()
                    .GroupBy(x => x.DeviceId)
                    .Select(x => new {DeviceId = x.Key, LocationsCount = x.Count()})
                    .ToListAsync();

                AssetResponseModel responseModel = mapper.Map<AssetEntity, AssetResponseModel>(entity);
                responseModel.Devices = devices.Select(x =>
                    {
                        DeviceModel deviceModel = mapper.Map<DeviceEntity, DeviceModel>(x);
                        deviceModel.LocationsCount = (locations.FirstOrDefault(y => y.DeviceId == x.Id)?.LocationsCount)
                            .GetValueOrDefault();
                        return deviceModel;
                    })
                    .ToList();

                return responseModel;
            }

            return null;
        }
    }
}
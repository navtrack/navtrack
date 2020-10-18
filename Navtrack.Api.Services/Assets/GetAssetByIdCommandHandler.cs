using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.CommandHandler;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(ICommandHandler<GetAssetByIdCommand, AssetModel>))]
    public class GetAssetByIdCommandHandler : BaseCommandHandler<GetAssetByIdCommand, AssetModel>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetAssetByIdCommandHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public override async Task<AssetModel> Handle(GetAssetByIdCommand command)
        {
            AssetEntity entity = await repository.GetEntities<AssetEntity>()
                .Include(x => x.Devices)
                .FirstOrDefaultAsync(x => x.Id == command.AssetId &&
                                          x.Users.Any(y => y.UserId == command.UserId));

            if (entity != null)
            {
                AssetModel model = mapper.Map<AssetEntity, AssetModel>(entity);

                model.Devices = await GetDevices(entity);

                return model;
            }

            return null;
        }

        private async Task<List<DeviceResponseModel>> GetDevices(AssetEntity entity)
        {
            var locationCount = await repository.GetEntities<LocationEntity>()
                .Where(x => x.AssetId == entity.Id)
                .GroupBy(x => x.DeviceId)
                .Select(x => new {DeviceId = x.Key, LocationsCount = x.Count()})
                .ToListAsync();


            return entity.Devices.Select(x =>
            {
                DeviceResponseModel deviceModel = mapper.Map<DeviceEntity, DeviceResponseModel>(x);
                deviceModel.LocationsCount = (locationCount.FirstOrDefault(y => y.DeviceId == x.Id)?.LocationsCount)
                    .GetValueOrDefault();

                return deviceModel;
            }).ToList();
        }
    }
}
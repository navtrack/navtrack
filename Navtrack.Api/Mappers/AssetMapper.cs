using Navtrack.Api.Models;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<AssetEntity, AssetModel>))]
    [Service(typeof(IMapper<AssetModel, AssetEntity>))]
    public class AssetMapper : IMapper<AssetEntity, AssetModel>, IMapper<AssetModel, AssetEntity>
    {
        private readonly IMapper mapper;

        public AssetMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public AssetModel Map(AssetEntity source, AssetModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.DeviceId = source.DeviceId;
            destination.Device = mapper.Map<DeviceEntity, DeviceModel>(source.Device);

            return destination;
        }

        public AssetEntity Map(AssetModel source, AssetEntity destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.DeviceId = source.DeviceId;

            return destination;
        }
    }
}
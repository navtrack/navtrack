using Navtrack.Api.Models;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<Asset, AssetModel>))]
    [Service(typeof(IMapper<AssetModel, Asset>))]
    public class AssetMapper : IMapper<Asset, AssetModel>, IMapper<AssetModel, Asset>
    {
        public AssetModel Map(Asset source, AssetModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.DeviceId = source.DeviceId;
            destination.DeviceType = $"{source.Device?.DeviceType?.Brand} {source.Device?.DeviceType?.Model}";

            return destination;
        }

        public Asset Map(AssetModel source, Asset destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.DeviceId = source.DeviceId;

            return destination;
        }
    }
}
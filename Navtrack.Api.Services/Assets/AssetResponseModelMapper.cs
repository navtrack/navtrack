using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IMapper<AssetEntity, AssetModel>))]
    [Service(typeof(IMapper<AssetModel, AssetEntity>))]
    public class AssetResponseModelMapper : IMapper<AssetEntity, AssetModel>, IMapper<AssetModel, AssetEntity>
    {
        public AssetModel Map(AssetEntity source, AssetModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;

            return destination;
        }

        public AssetEntity Map(AssetModel source, AssetEntity destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;

            return destination;
        }
    }
}
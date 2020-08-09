using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Assets
{
    [Service(typeof(IMapper<AssetEntity, AssetResponseModel>))]
    [Service(typeof(IMapper<AssetResponseModel, AssetEntity>))]
    public class AssetResponseModelMapper : IMapper<AssetEntity, AssetResponseModel>, IMapper<AssetResponseModel, AssetEntity>
    {
        public AssetResponseModel Map(AssetEntity source, AssetResponseModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;

            return destination;
        }

        public AssetEntity Map(AssetResponseModel source, AssetEntity destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;

            return destination;
        }
    }
}
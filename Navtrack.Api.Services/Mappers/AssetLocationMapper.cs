using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Locations;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<AssetEntity, LocationEntity, AssetResponseModel>))]
    public class AssetLocationMapper : IMapper<AssetEntity, LocationEntity, AssetResponseModel>
    {
        private readonly IMapper mapper;

        public AssetLocationMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public AssetResponseModel Map(AssetEntity source1, LocationEntity source2, AssetResponseModel destination)
        {
            mapper.Map(source1, destination);

            if (source2 != null)
            {
                destination.Location = mapper.Map<LocationEntity, LocationResponseModel>(source2);
            }
            
            return destination;
        }
    }
}
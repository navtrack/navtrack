using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<AssetEntity, LocationEntity, AssetModel>))]
    public class AssetLocationMapper : IMapper<AssetEntity, LocationEntity, AssetModel>
    {
        private readonly IMapper mapper;

        public AssetLocationMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public AssetModel Map(AssetEntity source1, LocationEntity source2, AssetModel destination)
        {
            mapper.Map(source1, destination);

            if (source2 != null)
            {
                destination.Location = mapper.Map<LocationEntity, LocationModel>(source2);
            }
            
            return destination;
        }
    }
}
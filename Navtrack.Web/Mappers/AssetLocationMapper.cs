using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<Asset, Location, AssetModel>))]
    public class AssetLocationMapper : IMapper<Asset, Location, AssetModel>
    {
        private readonly IMapper mapper;

        public AssetLocationMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public AssetModel Map(Asset source1, Location source2, AssetModel destination)
        {
            mapper.Map(source1, destination);

            if (source2 != null)
            {
                destination.Location = mapper.Map<Location, LocationModel>(source2);
            }
            
            return destination;
        }
    }
}
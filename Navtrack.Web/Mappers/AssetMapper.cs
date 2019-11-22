using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<Asset, AssetModel>))]
    public class AssetMapper : IMapper<Asset, AssetModel>
    {
        public AssetModel Map(Asset source, AssetModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;

            return destination;
        }
    }
}
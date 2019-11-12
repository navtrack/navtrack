using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Model.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<Object, AssetModel>))]
    public class AssetMapper : IMapper<Object, AssetModel>
    {
        public AssetModel Map(Object source, AssetModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;

            return destination;
        }
    }
}
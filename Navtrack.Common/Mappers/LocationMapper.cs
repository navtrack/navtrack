using Navtrack.Common.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Common.Mappers
{
    [Service(typeof(IMapper<Location, DataAccess.Model.Location>))]
    public class LocationMapper : IMapper<Location, DataAccess.Model.Location>
    {
        public DataAccess.Model.Location Map(Location source, DataAccess.Model.Location destination)
        {
            throw new System.NotImplementedException();
        }
    }
}
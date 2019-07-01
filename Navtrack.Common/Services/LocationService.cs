using System.Threading.Tasks;
using Navtrack.Common.DataServices;
using Navtrack.Common.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Common.Services
{
    [Service(typeof(ILocationService))]
    public class LocationService : ILocationService
    {
        private readonly ILocationDataService locationDataService;
        private readonly IMapper mapper;

        public LocationService(ILocationDataService locationDataService, IMapper mapper)
        {
            this.locationDataService = locationDataService;
            this.mapper = mapper;
        }

        public Task Add(Location location)
        {
            DataAccess.Model.Location mapped = mapper.Map<Location, DataAccess.Model.Location>(location);

            return locationDataService.Add(mapped);
        }
    }
}
using System.Threading.Tasks;
using Navtrack.Common.DataServices;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Location = Navtrack.Common.Model.Location;

namespace Navtrack.Common.Services
{
    [Service(typeof(ILocationService))]
    public class LocationService : ILocationService
    {
        private readonly ILocationDataService locationDataService;
        private readonly IObjectDataService objectDataService;
        private readonly IMapper mapper;

        public LocationService(ILocationDataService locationDataService, IMapper mapper,
            IObjectDataService objectDataService)
        {
            this.locationDataService = locationDataService;
            this.mapper = mapper;
            this.objectDataService = objectDataService;
        }

        public async Task Add(Location location)
        {
            Object @object = await objectDataService.GetObjectByIMEI(location.Device.IMEI);

            if (@object != null)
            {
                DataAccess.Model.Location mapped =
                    mapper.Map<Location, Object, DataAccess.Model.Location>(location, @object);

                await locationDataService.Add(mapped);
            }
        }
    }
}
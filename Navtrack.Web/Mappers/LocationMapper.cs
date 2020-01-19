using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;

namespace Navtrack.Web.Mappers
{
    [Service(typeof(IMapper<Location, LocationModel>))]
    public class LocationMapper : IMapper<Location, LocationModel>
    {
        public LocationModel Map(Location source, LocationModel destination)
        {
            destination.Id = source.Id;
            destination.Latitude = source.Latitude;
            destination.Longitude = source.Longitude;
            destination.DateTime = source.DateTime;
            destination.Speed = source.Speed;
            destination.Heading = source.Heading;
            destination.Altitude = source.Altitude;
            destination.Satellites = source.Satellites;
            destination.HDOP = source.HDOP;
            
            return destination;
        }
    }
}
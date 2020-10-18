using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Custom;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<LocationFilterRequestModel, LocationFilter>))]
    public class LocationFilterMapper : IMapper<LocationFilterRequestModel, LocationFilter>
    {
        public LocationFilter Map(LocationFilterRequestModel source, LocationFilter destination)
        {
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.MinAltitude = source.MinAltitude;
            destination.MaxAltitude = source.MaxAltitude;
            destination.MinSpeed = source.MinSpeed;
            destination.MaxSpeed = source.MaxSpeed;
            
            return destination;
        }
    }
}
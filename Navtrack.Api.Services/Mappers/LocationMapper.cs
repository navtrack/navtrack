using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<LocationEntity, LocationResponseModel>))]
    public class LocationMapper : IMapper<LocationEntity, LocationResponseModel>
    {
        public LocationResponseModel Map(LocationEntity source, LocationResponseModel destination)
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
            destination.PositionStatus = source.PositionStatus;
            destination.GsmSignal = source.GsmSignal;
            destination.Odometer = source.Odometer;
            
            return destination;
        }
    }
}
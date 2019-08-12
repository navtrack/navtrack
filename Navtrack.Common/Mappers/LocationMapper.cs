using System;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Location = Navtrack.Common.Model.Location;
using Object = Navtrack.DataAccess.Model.Object;

namespace Navtrack.Common.Mappers
{
    [Service(typeof(IMapper<Location, Object, DataAccess.Model.Location>))]
    public class LocationMapper : IMapper<Location, Object, DataAccess.Model.Location>
    {
        public DataAccess.Model.Location Map(Location source1, Object source2, DataAccess.Model.Location destination)
        {
            destination.Latitude = source1.Latitude;
            destination.Longitude = source1.Longitude;
            destination.DateTime = source1.DateTime;
            destination.Speed = source1.Speed;
            destination.Heading = source1.Heading;
            destination.Altitude = source1.Altitude;
            destination.DeviceId = source2.DeviceId;
            destination.ObjectId = source2.Id;
            destination.Satellites = source1.Satellites;
            destination.HDOP = source1.HDOP;
            destination.ProtocolData = source1.ProtocolData;
            destination.CreatedAt = DateTime.UtcNow;

            return destination;
        }
    }
}
using System;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Location = Navtrack.Listener.Models.Location;

namespace Navtrack.Listener.Mappers
{
    [Service(typeof(IMapper<Location, Asset, DataAccess.Model.Location>))]
    public class LocationMapper : IMapper<Location, Asset, DataAccess.Model.Location>
    {
        public DataAccess.Model.Location Map(Location source1, Asset source2, DataAccess.Model.Location destination)
        {
            destination.Latitude = source1.Latitude;
            destination.Longitude = source1.Longitude;
            destination.DateTime = source1.DateTime;
            destination.Speed = (int) source1.Speed; // TODO
            destination.Heading = (int) source1.Heading; // TODO
            destination.Altitude = source1.Altitude;
            destination.DeviceId = source2.DeviceId;
            destination.AssetId = source2.Id;
            destination.Satellites = source1.Satellites;
            destination.HDOP = source1.HDOP;
            destination.CreatedAt = DateTime.UtcNow;

            return destination;
        }
    }
}
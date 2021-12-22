using System;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Locations;
using Location = Navtrack.Listener.Models.Location;

namespace Navtrack.Listener.Mappers;

public static class LocationDocumentMapper
{
    public static LocationDocument Map(Location source1, AssetDocument source2, ObjectId connectionMessageId)
    {
        LocationDocument destination = new()
        {
            DeviceId = source2.Device.Id,
            AssetId = source2.Id,
            Coordinates = new[]
            {
                (double)source1.Longitude, (double)source1.Latitude
            },
            DateTime = source1.DateTime,
            Valid = source1.PositionStatus,
            Speed = source1.Speed as float?,
            Heading = source1.Heading as float?,
            Altitude = source1.Altitude as float?,
            Satellites = source1.Satellites,
            HDOP = source1.HDOP as float?,
            GsmSignal = source1.GsmSignal,
            Odometer = source1.Odometer,
            CreatedDate = DateTime.UtcNow,
            DeviceConnectionMessageId = connectionMessageId
        };

        if (source1.MobileCountryCode.HasValue || source1.MobileNetworkCode.HasValue ||
            source1.LocationAreaCode.HasValue || source1.CellId.HasValue)
        {
            destination.CellGlobalIdentity = new CellGlobalIdentityElement
            {
                MobileCountryCode = source1.MobileCountryCode,
                MobileNetworkCode = source1.MobileNetworkCode,
                LocationAreaCode = source1.LocationAreaCode,
                CellId = source1.CellId,
            };
        }

        return destination;
    }
}
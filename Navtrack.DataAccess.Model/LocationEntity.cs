using System;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class LocationEntity : CreatedEntityAudit, IEntity
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public decimal? Altitude { get; set; }
        public DeviceEntity Device { get; set; }
        public int DeviceId { get; set; }
        public AssetEntity Asset { get; set; }
        public int AssetId { get; set; }
        public short? Satellites { get; set; }
        public decimal? HDOP { get; set; }
        public bool? PositionStatus { get; set; }
        public short? GsmSignal { get; set; }
        public double? Odometer { get; set; }
        
        public int? MobileCountryCode { get; set; }
        public int? MobileNetworkCode { get; set; }
        public int? LocationAreaCode { get; set; }
        public int? CellId { get; set; }
        
        public int? DeviceConnectionMessageId { get; set; }
        public DeviceConnectionMessageEntity ConnectionMessage { get; set; }
    }
}
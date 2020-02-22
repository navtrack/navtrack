using System;

namespace Navtrack.Web.Models.Locations
{
    public class LocationHistoryRequestModel
    {
        public int AssetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? Radius { get; set; }
        public int? StartSpeed { get; set; }
        public int? EndSpeed { get; set; }
        public int? StartAltitude { get; set; }
        public int? EndAltitude { get; set; }
    }
}
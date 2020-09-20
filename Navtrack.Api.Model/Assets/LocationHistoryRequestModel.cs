using System;

namespace Navtrack.Api.Model.Assets
{
    public class LocationHistoryRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? StartSpeed { get; set; }
        public int? EndSpeed { get; set; }
        public int? StartAltitude { get; set; }
        public int? EndAltitude { get; set; }
    }
}
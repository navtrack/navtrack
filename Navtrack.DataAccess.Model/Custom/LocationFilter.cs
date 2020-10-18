using System;

namespace Navtrack.DataAccess.Model.Custom
{
    public class LocationFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MinSpeed { get; set; }
        public int? MaxSpeed { get; set; }
        public int? MinAltitude { get; set; }
        public int? MaxAltitude { get; set; }

        public bool HasFilters => StartDate.HasValue || EndDate.HasValue || 
                                  MinSpeed.HasValue || MaxSpeed.HasValue ||
                                  MinAltitude.HasValue || MaxAltitude.HasValue;
    }
}
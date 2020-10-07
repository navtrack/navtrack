using System.Collections.Generic;

namespace Navtrack.DataAccess.Model.Custom
{
    public class TripWithLocations
    {
        public TripEntity Trip { get; set; }
        public List<LocationEntity> Locations { get; set; }
    }
}
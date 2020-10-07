using System.Linq;
using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Assets
{
    public class GetTripsModel : ResultsModel<TripModel>
    {
        public int TotalDistance => Results.Sum(x => x.Distance);
        public int TotalLocations => Results.Sum(x => x.Locations.Count);
    }
}
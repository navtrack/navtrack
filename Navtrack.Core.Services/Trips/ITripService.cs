using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Core.Model.Trips;

namespace Navtrack.Core.Services.Trips;

public interface ITripService
{
    Task<IEnumerable<Trip>> GetTrips(string assetId, TripFilter tripFilter);
}
using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;

namespace Navtrack.Api.Services.Trips;

public interface ITripService
{
    Task<TripListModel> GetTrips(string assetId, TripFilterModel tripFilter);
}
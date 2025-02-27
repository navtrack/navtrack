using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.Trips;

public interface ITripService
{
    Task<TripList> GetTrips(AssetDocument asset, TripFilter tripFilter);
}
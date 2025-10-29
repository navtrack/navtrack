using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Services.Trips;

public interface ITripService
{
    Task<TripListModel> GetTrips(AssetEntity asset, TripFilterModel tripFilter);
}